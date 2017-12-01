using System.Net.Sockets;

namespace FileTransferManagerServer
{
    interface IFileManager
    {
        bool SendFile(string path, Socket socket);
        bool ReceiveFile(string path, Socket socket, long fileSize);
        bool DeleteFile(string path);
    }
}
