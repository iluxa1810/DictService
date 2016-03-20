using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DictService.DataAdapters;
using DictService.Helpers;

namespace DictService
{
    class Program
    {
        static void Main(string[] args)
        {
            SyncHelper.SyncDictService(Properties.Settings.Default.DictServiceConnStr);
        }
    }
}
