using System.IO;
using Android.App;

namespace MultitargetFileSystem.Droid
{
    public class FileSystem : IFileSystem
    {
        public string LocalStoragePath
            => Application.Context.GetExternalFilesDir(null).ToURI() + "/";

        public bool FileExists(string path)
            => File.Exists(path);

        public void WriteFile(string path, byte[] contents)
            => File.WriteAllBytes(path, contents);

        public void WriteFile(string path, string contents)
            => File.AppendAllText(path, contents);

        public string ReadFile(string path)
            => File.ReadAllText(path);

        public Stream GetInputStream(string path)
            => new FileStream(path, FileMode.Open, FileAccess.Read);

        public void DeleteFile(string path)
            => File.Delete(path);
    }
}
