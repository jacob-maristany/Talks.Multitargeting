using System.IO;

namespace MultitargetFileSystem
{
    public partial class FileSystem
    {
        private string PlatformLocalStoragePath 
            => Android.App.Application.Context.GetExternalFilesDir(null).ToURI() + "/";

        private bool PlatformFileExists(string path) => File.Exists(path);
        private void PlatformWriteFile(string path, byte[] contents) => File.WriteAllBytes(path, contents);
        private void PlatformWriteFile(string path, string contents) => File.AppendAllText(path, contents);
        private string PlatformReadFile(string path) => File.ReadAllText(path);
        private Stream PlatformGetInputStream(string path) => new FileStream(path, FileMode.Open, FileAccess.Read);
        private void PlatformDeleteFile(string path) => File.Delete(path);
    }
}