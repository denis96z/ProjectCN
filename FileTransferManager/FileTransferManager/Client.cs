using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public void UploadFile(string localPath, string serverPath)
        {
            throw new NotImplementedException();
        }

        public void DownloadFile(string serverPath, string localPath)
        {
            throw new NotImplementedException();
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

        public IEnumerable<string> GetFileList()
        {
            SendRequest("FILELIST");

            string response = ReceiveResponse();
            char delimiter = ' ';
            string[] args = response.Split(delimiter);

            if (args[0] == ResponseCode.OK)
            {
                yield break;
            }
            else if (args[0] == ResponseCode.OK_LIST)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    yield return args[i].Trim('"');
                }
            }
            else
            {
                throw new Exception(response);
            }
        }

        public IEnumerable<string> GetUserList()
        {
            SendRequest("USERLIST");

            string response = ReceiveResponse();
            char delimiter = ' ';
            string[] args = response.Split(delimiter);

            if (args[0] == ResponseCode.OK)
            {
                yield break;
            }
            else if (args[0] == ResponseCode.OK_LIST)
            {
                for (int i = 1; i < args.Length; i++)
                {
                    yield return args[i].Trim('"');
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
    }
}
