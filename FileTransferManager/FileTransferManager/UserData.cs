using System;

namespace FileTransferManager
{
    struct UserData
    {
        public string Login { get; private set; }
        public string State { get; private set; }

        public UserData(string login, string state)
        {
            Login = login;
            State = state;
        }

        public override string ToString()
        {
            return Login + (State == "Online" ?
                " (Online)" : String.Empty);
        }
    }
}
