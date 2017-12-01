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
    }
}
