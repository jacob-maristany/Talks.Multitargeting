using System;
using System.IO;
using Foundation;

namespace MultitargetFileSystem
{
    public partial class FileSystem
    {
        private string PlatformLocalStoragePath 
            => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        private bool PlatformFileExists(string path) 
            => NSFileManager.DefaultManager.FileExists(path);

        private void PlatformWriteFile(string path, byte[] contents)
        {
            DeleteFile(path);
            CreateFile(path, NSData.FromArray(contents));
        }

        private void PlatformWriteFile(string path, string contents)
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

        private string PlatformReadFile(string path)
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

        private Stream PlatformGetInputStream(string path)
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

        private void PlatformDeleteFile(string path)
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

            Serilog.Log.ForContext<FileSystem>().Information("File created: {Path}", path);
        }
    }
}