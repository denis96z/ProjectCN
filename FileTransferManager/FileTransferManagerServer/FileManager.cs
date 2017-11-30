using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferManager
{
    class FileManager : IFileManager
    {
        public bool DeleteFile(string path)
        {
            throw new NotImplementedException();
        }

        public bool RecvFile(string path, Socket socket, long fileSize)
        {
            throw new NotImplementedException();
        }

        public bool SendFile(string path, Socket socket)
        {
            throw new NotImplementedException();
        }
    }
}
