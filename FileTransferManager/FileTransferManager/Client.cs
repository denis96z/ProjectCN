using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileTransferManager
{
    class Client
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        private Socket socket = new Socket(AddressFamily
            .InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Client()
        {
            var ip = new List<IPAddress>(from addr
                in Dns.GetHostAddresses(ServerAddress.SERVER_DNS)
                                         where addr.AddressFamily == AddressFamily.InterNetwork
                                         select addr)[0];
            socket.Connect(new IPEndPoint(ip, ServerAddress.SERVER_PORT));
        }

        public void Register(string login, string password)
        {
            SendRequest("REGISTER \"" + login + "\" \"" + password + "\"");
            string response = ReceiveResponse();
            if (response == ResponseCode.OK)
            {
                Login = login;
                Password = password;
            }
            else
            {
                throw new Exception(response);
            }
        }

        public void SignIn(string login, string password)
        {
            SendRequest("SIGNIN \"" + login + "\" \"" + password + "\"");
            string response = ReceiveResponse();
            if (response == ResponseCode.OK)
            {
                Login = login;
                Password = password;
            }
            else
            {
                throw new Exception(response);
            }
        }

        public void Disconnect()
        {
            SendRequest("DISCONNECT");
            string response = ReceiveResponse();

            if (response == ResponseCode.OK)
            {
                socket.Close();
                socket = null;
            }
            else
            {
                throw new Exception(response);
            }
        }

        public delegate void FileTransferStartCallBack(long fileSize);
        public delegate void FileTransferProgressCallback(long transferedSize);

        public void UploadFile(string localPath, string serverPath,
            OnFileTransterProgress onProgress)
        {
            FileInfo fileInfo = new FileInfo(localPath);

            SendRequest("UPLOAD \"" + serverPath + "\" \"" + fileInfo.Length + "\"");
            string response = ReceiveResponse();
            if (response != ResponseCode.OK)
            {
                throw new Exception(response);
            }

            SendFile(localPath, socket, onProgress);
            response = ReceiveResponse();
            if (response != ResponseCode.OK)
            {
                throw new Exception(response);
            }
        }

        public void DownloadFile(string serverPath, string owner,
            string localPath, OnFileTransterProgress onProgress)
        {
            string request = "DOWNLOAD \"" + serverPath + "\" \"" + owner + "\"";
            SendRequest(request);

            string response = ReceiveResponse();
            string[] args = response.Split(' ');
            if (args[0] != ResponseCode.OK_LIST)
            {
                throw new Exception(response);
            }

            long size = long.Parse(args[1].Trim('"'));
            SendRequest(ResponseCode.OK);

            ReceiveFile(localPath, socket, size, onProgress);
        }

        public void Share(string serverPath, string user)
        {
            SendRequest("SHARE \"" + serverPath + "\" \"" + user + "\"");
            string response = ReceiveResponse();
            if (response != ResponseCode.OK)
            {
                throw new Exception(response);
            }
        }

        public IEnumerable<FileData> GetFileList()
        {
            SendRequest("FILELIST");

            string response = ReceiveResponse();
            string[] args = response.Split(' ', '\0');

            if (args[0] == ResponseCode.OK)
            {
                yield break;
            }
            else if (args[0] == ResponseCode.OK_LIST)
            {
                for (int i = 1; i < args.Length - 3; i += 4)
                {
                    yield return new FileData(
                        args[i].Trim('"'), args[i + 1].Trim('"'),
                        long.Parse(args[i + 3].Trim('"')));
                }
            }
            else
            {
                throw new Exception(response);
            }
        }

        public IEnumerable<UserData> GetUserList()
        {
            SendRequest("USERLIST");

            string response = ReceiveResponse();
            string[] args = response.Split(' ', '\0');

            if (args[0] == ResponseCode.OK)
            {
                yield break;
            }
            else if (args[0] == ResponseCode.OK_LIST)
            {
                for (int i = 1; i < args.Length - 1; i += 2)
                {
                    yield return new UserData(args[i].Trim('"'), args[i + 1].Trim('"'));
                }
            }
            else
            {
                throw new Exception(args[0]);
            }
        }

        public void SignOut()
        {
            SendRequest("SIGNOUT");
            string response = ReceiveResponse();

            if (response != ResponseCode.OK)
            {
                throw new Exception(response);
            }

            Login = null;
            Password = null;
        }

        private string ReceiveResponse()
        {
            byte[] buffer = new byte[100];
            var requestBuilder = new StringBuilder();
            do
            {
                int length = socket.Receive(buffer);
                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, length));
            }
            while (socket.Available > 0);
            string request = requestBuilder.ToString();

            return request;
        }

        private void SendRequest(string response)
        {
            socket.Send(Encoding.UTF8.GetBytes(response));
        }

        private const int BUFFER_SIZE = 1000;

        public delegate void OnFileTransterProgress(long progress, long fileSize);

        public void SendFile(string path, Socket socket, OnFileTransterProgress onProgress)
        {
            var fileStream = File.OpenRead(path);
            using (var reader = new BinaryReader(fileStream))
            {
                long numFullBuffers = reader.BaseStream.Length / BUFFER_SIZE;
                int restBufferSize = (int)(reader.BaseStream.Length % BUFFER_SIZE);

                for (long i = 1; i <= numFullBuffers; i++)
                {
                    var buffer = reader.ReadBytes(BUFFER_SIZE);
                    socket.Send(buffer);

                    onProgress.Invoke(i * BUFFER_SIZE, reader.BaseStream.Length);
                }

                if (restBufferSize > 0)
                {
                    var buffer = reader.ReadBytes(restBufferSize);
                    socket.Send(buffer);

                    onProgress.Invoke(reader.BaseStream.Length, reader.BaseStream.Length);
                }

                reader.Close();
            }
        }

        public void ReceiveFile(string path, Socket socket,
            long fileSize, OnFileTransterProgress onProgress)
        {
            var fileStream = File.Create(path);
            using (var writer = new BinaryWriter(fileStream))
            {
                long recvCounter = 0;
                byte[] buffer = new byte[BUFFER_SIZE];
                while (recvCounter < fileSize)
                {
                    int length = socket.Receive(buffer);
                    for (int i = 0; i < length; i++)
                    {
                        writer.Write(buffer[i]);
                    }
                    recvCounter += length;
                    onProgress.Invoke(recvCounter, fileSize);
                }
                writer.Close();
            }
        }
    }
}
