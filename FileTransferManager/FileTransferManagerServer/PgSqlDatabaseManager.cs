using System.Text;
using Devart.Data.PostgreSql;

namespace FileTransferManagerServer
{
    class PgSqlDatabaseManager : DatabaseManager
    {
        private PgSqlConnection connection =
            new PgSqlConnection(CONNECTION_STRING);

        public override void Register(string login, string password)
        {
            string commandText = CreateCommand("Register", login, password);
            ExecuteNonQuery(commandText);
        }

        public override void SignIn(string login, string password)
        {
            string commandText = CreateCommand("SignIn", login, password);
            ExecuteNonQuery(commandText);
        }

        public override void SignOut(string login, string password)
        {
            string commandText = CreateCommand("SignOut", login, password);
            ExecuteNonQuery(commandText);
        }

        public override void AddFile(string login, string password,
            string rootDir, string path, long size)
        {
            string commandText = CreateCommand("AddFile",
                login, password, rootDir, path, size);
            ExecuteNonQuery(commandText);
        }

        public override void DeleteFile(string login, string password,
            string path, string owner)
        {
            string commandText = CreateCommand("DeleteFile",
                login, password, path, owner);
            ExecuteNonQuery(commandText);
        }

        public override void ShareFile(string login, string password,
            string path, string user)
        {
            string commandText = CreateCommand("ShareFile",
                login, password, path, user);
            ExecuteNonQuery(commandText);
        }

        public override string GetVirtualFileName(string login,
            string password, string path, string owner)
        {
            string commandText = CreateCommand("GetVirtualFileName",
                login, password, path, owner);
            return ExecuteReturnString(commandText);
        }

        public override string GetUsersList(string login, string password)
        {
            string commandText = CreateCommand("GetUsersList", login, password);
            return ExecuteReturnList(commandText, 2);
        }

        public override string GetFilesList(string login, string password)
        {
            string commandText = CreateCommand("GetFilesList", login, password);
            return ExecuteReturnList(commandText, 4);
        }

        private string CreateCommand(string function, params object[] args)
        {
            string command = "select * from \"" + function + "\"(";
            if (args.Length > 0)
            {
                int lastIndex = args.Length - 1;
                for (int i = 0; i < lastIndex; i++)
                {
                    command += CreateArgument(args[i]) + ", ";
                }
                command += CreateArgument(args[lastIndex]);
            }
            command += ");";
            return command;
        }

        private string CreateArgument(object arg)
        {
            return arg is string ? "'" + arg.ToString() + "'" : arg.ToString();
        }

        private void ExecuteNonQuery(string commandText)
        {
            PgSqlCommand command = new PgSqlCommand(commandText, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }

        private string ExecuteReturnString(string commandText)
        {
            PgSqlCommand command = new PgSqlCommand(commandText, connection);
            connection.Open();
            var reader = command.ExecuteReader();
            reader.Read();
            string result = reader.GetString(0);
            reader.Close();
            connection.Close();
            return result;
        }

        private string ExecuteReturnList(string commandText, int numColumns)
        {
            PgSqlCommand command = new PgSqlCommand(commandText, connection);
            connection.Open();

            StringBuilder result = new StringBuilder();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < numColumns; i++)
                    {
                        result.Append(" " + reader.GetValue(i).ToString());
                    }
                }
                reader.Close();
            }                

            connection.Close();
            return result.ToString().TrimStart(' ');
        }
    }
}
