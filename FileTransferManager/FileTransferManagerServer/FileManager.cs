using NLog;
using System;
using System.IO;
using System.Net.Sockets;

namespace FileTransferManagerServer
{
    class FileManager : IFileManager
    {
        private const int BUFFER_SIZE = 100;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public bool SendFile(string path, Socket socket)
        {
            try
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
                    }

                    if (restBufferSize > 0)
                    {
                        var buffer = reader.ReadBytes(restBufferSize);
                        socket.Send(buffer);
                    }

                    reader.Close();
                }
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Tried to send \"" + path +
                    "\" to \"" + socket.RemoteEndPoint.ToString() + "\".");
                return false;
            }
        }

        public bool ReceiveFile(string path, Socket socket, long fileSize)
        {
            try
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
                    }
                    writer.Close();
                }
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Tried to receive \"" + path +
                    "\" from \"" + socket.RemoteEndPoint.ToString() + "\".");
                return false;
            }
        }

        public bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Tried to delete \"" + path + "\".");
                return false;
            }
        }
    }
}
