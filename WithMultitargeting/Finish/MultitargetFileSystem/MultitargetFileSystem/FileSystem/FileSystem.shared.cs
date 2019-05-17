using System.IO;

namespace MultitargetFileSystem
{
    public partial class FileSystem
    {
        public string LocalStoragePath => PlatformLocalStoragePath;

        public bool FileExists(string path) => PlatformFileExists(path);
        public void WriteFile(string path, byte[] contents) => PlatformWriteFile(path, contents);
        public void WriteFile(string path, string contents) => PlatformWriteFile(path, contents);
        public string ReadFile(string path) => PlatformReadFile(path);
        public Stream GetInputStream(string path) => PlatformGetInputStream(path);
        public void DeleteFile(string path) => PlatformDeleteFile(path);
    }
}