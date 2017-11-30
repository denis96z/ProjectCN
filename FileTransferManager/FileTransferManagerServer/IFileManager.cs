using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferManager
{
    interface IFileManager
    {
        bool RecvFile(string path, Socket socket, long fileSize);
        bool SendFile(string path, Socket socket);
        bool DeleteFile(string path);
    }
}
