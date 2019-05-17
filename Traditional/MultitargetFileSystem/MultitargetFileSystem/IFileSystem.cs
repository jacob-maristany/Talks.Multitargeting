using System.IO;

namespace MultitargetFileSystem
{
    public interface IFileSystem
    {
        string LocalStoragePath { get; }

        bool FileExists(string path);
        void WriteFile(string path, byte[] contents);
        void WriteFile(string path, string contents);
        string ReadFile(string path);
        Stream GetInputStream(string path);
        void DeleteFile(string path);
    }
}
