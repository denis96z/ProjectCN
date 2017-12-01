using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using NLog;
using ServerAddress = FileTransferManager.ServerAddress;

namespace FileTransferManagerServer
{
    class Server
    {
        private TcpListener tcpListener =
            new TcpListener(IPAddress.Any, ServerAddress.SERVER_PORT);

        private Logger logger = LogManager.GetCurrentClassLogger();

        public void StartServer()
        {
            new Thread(() =>
            {
                try
                {
                    tcpListener.Start();
                    logger.Debug("Server started.");

                    while (true)
                    {
                        var client = tcpListener.AcceptTcpClient();
                        logger.Debug("Accepted connection " +
                            client.Client.RemoteEndPoint + ".");

                        new Thread(() => new UserManager(client.Client)
                            .HandleRequests()).Start();
                    }
                }
                catch
                {
                    logger.Debug("Server stopped.");
                }
            }).Start();
        }

        public void StopServer()
        {
            tcpListener.Stop();
        }
    }
}
