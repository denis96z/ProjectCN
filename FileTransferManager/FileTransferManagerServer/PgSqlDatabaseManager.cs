using System;
using Devart.Data.PostgreSql;

namespace FileTransferManagerServer
{
    class PgSqlDatabaseManager : DatabaseManager
    {
        private PgSqlConnection connection =
            new PgSqlConnection(CONNECTION_STRING);

        public void Register(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void SignIn(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void SignOut(string login, string password)
        {
            throw new NotImplementedException();
        }

        public void AddFile(string login, string password,
            string rootDir, string path, long size)
        {
            throw new NotImplementedException();
        }

        public void DeleteFile(string login, string password,
            string path, string owner)
        {
            throw new NotImplementedException();
        }

        public void ShareFile(string login, string password,
            string path, string user)
        {
            throw new NotImplementedException();
        }

        public string GetVirtualFileName(string login,
            string password, string path, string owner)
        {
            throw new NotImplementedException();
        }

        public string GetUsersList(string login, string password)
        {
            throw new NotImplementedException();
        }

        public string GetFilesList(string login, string password)
        {
            throw new NotImplementedException();
        }

        private PgSqlCommand ParseCommand(string command)
        {
            throw new NotImplementedException();
        }

        private void ExecuteNonQuery(string command)
        {
            throw new NotImplementedException();
        }
    }
}
