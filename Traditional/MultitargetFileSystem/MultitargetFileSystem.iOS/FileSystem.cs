using System;
using System.IO;
using Foundation;

namespace MultitargetFileSystem.iOS
{
    public class FileSystem : IFileSystem
    {
        public string LocalStoragePath
            => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        public bool FileExists(string path) 
            => NSFileManager.DefaultManager.FileExists(path);

        public void WriteFile(string path, byte[] contents)
        {
            DeleteFile(path);
            CreateFile(path, NSData.FromArray(contents));
        }

        public void WriteFile(string path, string contents)
        {
            if (FileExists(path))
            {
                NSFileHandle handle = NSFileHandle.OpenUpdateUrl(NSUrl.CreateFileUrl(new[] { path }), out NSError error);

                if (error != null)
                {
                    throw new NSErrorException(error);
                }
                else
                {
                    handle.SeekToEndOfFile();
                    handle.WriteData(NSData.FromString(contents));
                    handle.CloseFile();
                }
            }
            else
            {
                CreateFile(path, NSData.FromString(contents));
            }
        }

        public string ReadFile(string path)
        {
            if (FileExists(path))
            {
                NSFileHandle handle = NSFileHandle.OpenReadUrl(NSUrl.CreateFileUrl(new[] { path }), out NSError error);

                if (error != null)
                {
                    throw new NSErrorException(error);
                }
                else
                {
                    NSString result = NSString.FromData(handle.ReadDataToEndOfFile(), NSStringEncoding.UTF8);
                    handle.CloseFile();
                    return result?.ToString() ?? "";
                }
            }

            throw new FileNotFoundException();
        }

        public Stream GetInputStream(string path)
        {
            if (FileExists(path))
            {
                NSFileHandle handle = NSFileHandle.OpenReadUrl(NSUrl.CreateFileUrl(new[] { path }), out NSError error);

                if (error != null)
                {
                    throw new NSErrorException(error);
                }
                else
                {
                    NSData result = handle.ReadDataToEndOfFile();
                    handle.CloseFile();
                    return result.AsStream();
                }
            }

            throw new FileNotFoundException();
        }

        public void DeleteFile(string path)
        {
            if (FileExists(path))
            {
                NSFileManager.DefaultManager.Remove(NSUrl.CreateFileUrl(new[] { path }), out NSError error);

                if (error != null)
                {
                    throw new NSErrorException(error);
                }
            }
        }

        private void CreateFile(string path, NSData data)
        {
            var dict = new NSMutableDictionary();
            var protection = new NSString("NSFileProtectionCompleteUnlessOpen");
            dict.Add(new NSString("NSFileProtectionKey") as NSObject, protection as NSObject);

            NSFileManager.DefaultManager.CreateFile(path, data, dict);

            Serilog.Log.ForContext<IFileSystem>().Information("File created: {Path}", path);
        }
    }
}
