using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCFTestConsoleAPP.DictService;
using Common.Helpers;


namespace WCFTestConsoleAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            //Thread.Sleep(1000);
            //Download();
            ZipHelper.CheckZipFile(@"D:\Dictionary\TMP\images.zip");
            //zip.CreateZipDictionary(@"D:\TestZip", @"D:\KP097R_R206_18_1CONV.mdb");
        }

        static void Download()
        {
            var client=new FileDownloadClient();
            Stream file;
            client.Open();
            var filename = client.Download("iluxa1810", 2,out file);
           Download(file, @"D:\Dictionary\Download\"+ filename);

        }
        static void AddNew()
        {
            string filePath = @"D:\KP097R_R206_18_1CONV.mdb";
            Stream file = new FileStream(filePath, FileMode.Open);
            var dict = new DictionaryInfo
            {
                // Dictionary_id =,
                Category_id = 1,
                Description = "Тестовый словарь",
                FriendlyName = "Словарь1",
                FileName = "KP097R_R206_18_1CONV.mdb",
                Action = ActionEnum.AddDict,
                SenderLogin = "iluxa1810"
            };
            FileUploadClient client = new FileUploadClient();
            Thread.Sleep(1000);
            client.Open();
            try
            {
                client.Upload(dict, file);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
            client.Close();
        }

       static void Download(Stream stream, string uploadPath)
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
            using (FileStream outputStream = new FileStream(uploadPath,
                FileMode.Create, FileAccess.Write))
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
                outputStream.Close();
            }
        }
    }
}
