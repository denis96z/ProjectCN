namespace FileTransferManagerServer
{
    abstract class DatabaseManager
    {
        private const string USER_ID = "postgres";
        private const string PASSWORD = "0000";
        private const string HOST = "localhost";
        private const string PORT = "5432";
        private const string DATABASE = "FileTransfer";

        protected const string CONNECTION_STRING =
            "User Id=" + USER_ID + ";" +
            "Password=" + PASSWORD + ";" +
            "Host=" + HOST + ";" +
            "Port=" + PORT + ";" +
            "Database=" + DATABASE + ";";

        public abstract void Register(string login, string password);
        public abstract void SignIn(string login, string password);
        public abstract void SignOut(string login, string password);

        public abstract void AddFile(string login,
            string password, string rootDir, string path, long size);
        public abstract void DeleteFile(string login,
            string password, string path, string owner);
        public abstract void ShareFile(string login,
            string password, string path, string user);

        public abstract string GetVirtualFileName(string login,
            string password, string path, string owner);
        public abstract long GetFileSize(string login,
            string password, string path, string owner);

        public abstract string GetUsersList(string login, string password);
        public abstract string GetFilesList(string login, string password);
    }
}
