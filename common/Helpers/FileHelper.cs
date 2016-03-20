using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class FileHelper
    {
        public static async Task LoadFileFromStreamAsync(Stream stream, string uploadPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(uploadPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));
            }
            if (File.Exists(uploadPath))
            {
                File.Delete(uploadPath);
            }
            const int bufferSize = 2048;
            byte[] buffer = new byte[bufferSize];
            using (FileStream outputStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);
                    while (bytesRead > 0)
                    {
                        await outputStream.WriteAsync(buffer, 0, bytesRead);
                        bytesRead = await stream.ReadAsync(buffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public static string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }
        public static void LoadFileFromStream(Stream stream, string uploadPath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(uploadPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));
            }
            if (File.Exists(uploadPath))
            {
                File.Delete(uploadPath);
            }

            const int bufferSize = 2048;
            byte[] buffer = new byte[bufferSize];
            using (FileStream outputStream = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, bufferSize);
                    while (bytesRead > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                        bytesRead = stream.Read(buffer, 0, bufferSize);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
        public static void DeleteFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);
            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }
            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                DeleteFolder(di.FullName);
                di.Delete();
            }
            dir.Delete();
        }
    }
}
