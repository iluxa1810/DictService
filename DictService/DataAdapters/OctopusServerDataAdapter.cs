using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DictService.Classes;

namespace DictService.DataAdapters
{
    class OctopusServerDataAdapter
    {
        public string ConnectionString { get; set; }

        public OctopusServerDataAdapter(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
        }

        public List<OctopusServer> GetAll()
        {
            string cmd = @"select * from dbo.[OctopusServer]";
            using (var conn = new SqlConnection(ConnectionString))
            {
                return (List<OctopusServer>) conn.Query<OctopusServer>(cmd);
            }
        } 
    }
}
