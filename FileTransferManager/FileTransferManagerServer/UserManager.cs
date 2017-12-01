using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using NLog;
using ResponseCode = FileTransferManager.ResponseCode;

namespace FileTransferManagerServer
{
    class UserManager
    {
        private readonly Socket socket;

        private IFileManager fileManager = new FileManager();
        private DatabaseManager databaseManager = new PgSqlDatabaseManager();

        private string login = null, password = null;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public UserManager(Socket clientSocket)
        {
            socket = clientSocket;
        }

        public void HandleRequests()
        {
            try
            {
                while (true)
                {
                    var request = ReceiveRequest();

                    var requestParts = ParseRequest(request);
                    switch (requestParts[0])
                    {
                        case "DISCONNECT":
                            if (requestParts.Count != 1)
                            {
                                SendResponseBadRequest();
                            }
                            else
                            {
                                SendResponseShutdown(ResponseCode.OK);
                            }
                            break;

                        case "REGISTER":
                            HandleRegisterRequest(requestParts);
                            break;

                        case "SIGNIN":
                            HandleSignInRequest(requestParts);
                            break;

                        case "SIGNOUT":
                            HandleSignOutRequest(requestParts);
                            break;

                        case "UPLOAD":
                            HandleUploadRequest(requestParts);
                            break;

                        case "DOWNLOAD":
                            HandleDownloadRequest(requestParts);
                            break;

                        case "DELETE":
                            HandleDeleteRequest(requestParts);
                            break;

                        case "SHARE":
                            HandleShareRequest(requestParts);
                            break;

                        case "USERLIST":
                            HandleUserListRequest(requestParts);
                            break;

                        case "FILELIST":
                            HandleFileListRequest(requestParts);
                            break;

                        default:
                            SendResponseBadRequest();
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                if (login != null)
                {
                    databaseManager.SignOut(login, password);
                }
            }
        }

        #region ProtocolMethods

        private void HandleRegisterRequest(List<string> requestParts)
        {
            if (requestParts.Count != 3)
            {
                SendResponseBadRequest();
            }
            else if (login != null)
            {
                SendResponseAlreadyAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    databaseManager.Register(requestParts[1], requestParts[2]);
                    databaseManager.SignIn(requestParts[1], requestParts[2]);
                    login = requestParts[1];
                    password = requestParts[2];
                    SendResponseOk();
                });
            }
        }

        private void HandleSignInRequest(List<string> requestParts)
        {
            if (requestParts.Count != 3)
            {
                SendResponseBadRequest();
            }
            else if (login != null)
            {
                SendResponseAlreadyAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    databaseManager.SignIn(requestParts[1], requestParts[2]);
                    login = requestParts[1];
                    password = requestParts[2];
                    SendResponseOk();
                });
            }
        }

        private void HandleSignOutRequest(List<string> requestParts)
        {
            if (requestParts.Count != 1)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    databaseManager.SignOut(login, password);
                    login = null;
                    password = null;
                    SendResponseOk();
                });
            }
        }

        private void HandleUploadRequest(List<string> requestParts)
        {
            if (requestParts.Count != 3)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                bool fileAdded = false;
                try
                {
                    string path = requestParts[1];
                    long size = long.Parse(requestParts[2]);

                    databaseManager.AddFile(login, password,
                        ServerData.RootDirectory, login, size);
                    fileAdded = true;

                    string virtualPath = databaseManager
                        .GetVirtualFileName(login, password, path, login);
                    fileManager.ReceiveFile(path, socket, size);

                    string hsRequest = ReceiveRequest();
                    if (hsRequest == "HANDSHAKE")
                    {
                        SendResponseOk();
                    }
                    else
                    {
                        SendResponseBadRequest();
                    }
                }
                catch (Exception exception)
                {
                    logger.Error(exception);

                    if (socket.Connected)
                    {
                        SendResponseError(exception.Message);
                    }
                    else if (login != null)
                    {
                        databaseManager.SignOut(login, password);
                    }

                    if (fileAdded)
                    {
                        databaseManager.DeleteFile(login, password, requestParts[1], login);
                    }
                }
            }
        }

        private void HandleDownloadRequest(List<string> requestParts)
        {
            if (requestParts.Count != 3)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    string path = requestParts[1];
                    string owner = requestParts[2];

                    string virtualPath = databaseManager
                        .GetVirtualFileName(login, password, path, owner);
                    fileManager.SendFile(path, socket);

                    string hsRequest = ReceiveRequest();
                    if (hsRequest == "HANDSHAKE")
                    {
                        SendResponseOk();
                    }
                    else
                    {
                        SendResponseBadRequest();
                    }
                });
            }
        }

        private void HandleDeleteRequest(List<string> requestParts)
        {
            if (requestParts.Count != 3)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    string path = requestParts[1];
                    string owner = requestParts[2];
                    databaseManager.DeleteFile(login, password, path, owner);
                    SendResponseOk();
                });
            }
        }

        private void HandleShareRequest(List<string> requestParts)
        {
            if (requestParts.Count != 3)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    string path = requestParts[1];
                    string user = requestParts[2];
                    databaseManager.ShareFile(login, password, path, user);
                    SendResponseOk();
                });
            }
        }

        private void HandleUserListRequest(List<string> requestParts)
        {
            if (requestParts.Count != 1)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    var list = databaseManager.GetUsersList(login, password);
                    SendResponseOkList(list);
                });
            }
        }

        private void HandleFileListRequest(List<string> requestParts)
        {
            if (requestParts.Count != 1)
            {
                SendResponseBadRequest();
            }
            else if (login == null)
            {
                SendResponseNotAuthorized();
            }
            else
            {
                HandleSecure(() =>
                {
                    var list = databaseManager.GetFilesList(login, password);
                    SendResponseOkList(list);
                });
            }
        }

        #endregion

        #region SpecialMethods

        private List<string> ParseRequest(string request)
        {
            Regex regex = new Regex("^(\\w+)( \"[^ \\f\\n\\r\\t\\v\"\']+\")*$");
            if (regex.IsMatch(request))
            {
                var args = request.Split(' ', '"');

                var parts = new List<string>();
                foreach (var arg in args)
                {
                    if (arg != String.Empty) parts.Add(arg);
                }

                return parts;
            }
            else
            {
                return null;
            }
        }

        private void HandleSecure(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                logger.Error(exception);

                if (socket.Connected)
                {
                    SendResponseError(exception.Message);
                }
                else if (login != null)
                {
                    databaseManager.SignOut(login, password);
                }
            }
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

            logger.Debug("Received request from " +
                socket.RemoteEndPoint + ": " + request);

            return request;
        }

        private void SendResponseOk()
        {
            SendResponse(ResponseCode.OK);
        }

        private void SendResponseOkList(string list)
        {
            SendResponse(ResponseCode.OK_LIST + " " + list);
        }

        private void SendResponseError()
        {
            SendResponse(ResponseCode.ERROR);
        }

        private void SendResponseError(string explanation)
        {
            SendResponse(ResponseCode.ERROR_EXPL + " \"" + explanation + "\"");
        }

        private void SendResponseBadRequest()
        {
            SendResponseError("Bad request.");
        }

        private void SendResponseAlreadyAuthorized()
        {
            SendResponseError("Already authorized.");
        }

        private void SendResponseNotAuthorized()
        {
            SendResponseError("Not authorized.");
        }

        private void SendResponse(string response)
        {
            socket.Send(Encoding.UTF8.GetBytes(response));
            logger.Debug("Sent response to " +
                socket.RemoteEndPoint + ": " + response);
        }

        private void SendResponseShutdown(string response)
        {
            SendResponse(response);
            socket.Close();
        }

        #endregion
    }
}
