using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferManagerServer
{
    static class ServerData
    {
        private static volatile string rootDirectory =
            Environment.CurrentDirectory + "\\storage";

        public static string RootDirectory
        {
            get { return rootDirectory; }
            set { rootDirectory = value; }
        }
    }
}
