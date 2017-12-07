using System;

namespace FileTransferManager
{
    struct FileData
    {
        public string Path { get; private set; }
        public string Owner { get; private set; }
        public long Size { get; private set; }

        public FileData(string path, string owner, long size)
        {
            Path = path;
            Owner = owner;
            Size = size;
        }

        public override string ToString()
        {
            return Path + " (owner: " + Owner + ")";
        }
    }
}
