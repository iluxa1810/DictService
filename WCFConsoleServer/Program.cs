using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using WCFConsoleServer.Services;

namespace Common.DictServiceEnums.DictServiceEnums
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(DictionaryService), new Uri("http://localhost:8080")))
            {
                host.Open();
                Console.WriteLine("Хост запущен");
                Console.ReadKey();
                host.Close();
            }
        }
    }
}
