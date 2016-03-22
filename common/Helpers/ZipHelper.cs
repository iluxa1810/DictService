using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;

namespace Common.Helpers
{
    public class ZipHelper
    {
        public static string CreateZipDictionary(string filePath)
        {
            using (var zipFile = new ZipFile(Encoding.GetEncoding(866)))
            {
                zipFile.CompressionLevel = CompressionLevel.BestCompression;
                zipFile.AddFile(filePath, "");
                var zipPath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(filePath)}.zip");
                zipFile.Save(zipPath);
                return zipPath;
            }
        }
        public static string CreateZipDictionary(string srcFile, string dstDir)
        {
            using (var zipFile = new ZipFile(Encoding.GetEncoding(866)))
            {
                zipFile.CompressionLevel = CompressionLevel.BestCompression;
                zipFile.AddFile(srcFile,"");
                if (!Directory.Exists(dstDir))
                {
                    Directory.CreateDirectory(dstDir);
                }
                var dstFile = Path.Combine(dstDir ,Path.GetFileNameWithoutExtension(srcFile) + ".zip");
                zipFile.Save(dstFile);
                return dstFile;
            }
        }

        public static bool CheckZipFile(string pathToZip)
        {
            try
            {
                using (var zipFile = new ZipFile(pathToZip))
                {

                    zipFile.ExtractAll(Path.GetDirectoryName(pathToZip));
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static void AddDictionaryToZip(string pathToZip, string filePath)
        {
            using (var zipFile = new ZipFile(pathToZip,Encoding.GetEncoding(866)))
            {
                zipFile.AddFile(filePath, "");
                zipFile.Save();
            }
        }

        public static void UnZip(string zipPath)
        {
            using (var zipFile = new ZipFile(zipPath, Encoding.GetEncoding(866)))
            {
                zipFile.ExtractAll(Path.GetDirectoryName(zipPath));
            }
        }
        public static void UnZip(string zipPath,string dstDir)
        {
            using (var zipFile = new ZipFile(zipPath,Encoding.GetEncoding(866)))
            {
                zipFile.EntryFileNames.Single();
                zipFile.ExtractAll(dstDir);
            }
        }

        public static string UnZipToTempDir(string zipPath)
        {
            string dstDir;
            using (var zipFile = new ZipFile(zipPath, Encoding.GetEncoding(866)))
            {
                dstDir = FileHelper.GetTemporaryDirectory();
                zipFile.ExtractAll(dstDir);
            }
            return dstDir;
        }
    }
}
