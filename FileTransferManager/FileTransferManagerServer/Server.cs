using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ServerAddress = FileTransferManager.ServerAddress;

namespace FileTransferManagerServer
{
    class Server
    {
        private TcpListener tcpListener =
            new TcpListener(IPAddress.Any, ServerAddress.SERVER_PORT);

        public void StartServer()
        {
            new Thread(() =>
            {
                try
                {
                    tcpListener.Start();

                    while (true)
                    {
                        var client = tcpListener.AcceptTcpClient();
                        new Thread(() => new UserManager(client.Client)
                            .HandleRequests()).Start();
                    }
                }
                catch
                {
                    
                }
            }).Start();
        }

        public void StopServer()
        {
            tcpListener.Stop();
        }
    }
}
