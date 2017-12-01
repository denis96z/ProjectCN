using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace FileTransferManagerServer
{
    class UserManager
    {
        private readonly Socket socket;

        private string login = null, password = null;

        public UserManager(Socket clientSocket)
        {
            socket = clientSocket;
        }

        public void HandleRequests()
        {
            throw new NotImplementedException();
        }

        #region ProtocolMethods

        private void HandleRegisterRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleSignInRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleSignOutRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleUploadRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleDownloadRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleDeleteRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleShareRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleUserlistRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        private void HandleFileListRequest(List<string> requestParts)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region SocketMethods

        private string ReceiveRequest()
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

        private void SendResponse(string response)
        {
            socket.Send(Encoding.UTF8.GetBytes(response));
        }

        private void SendResponseShutdown(string response)
        {
            SendResponse(response);
            socket.Close();
        }

        #endregion
    }
}
