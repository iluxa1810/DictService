using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictService.Classes;
using DictService.DataAdapters;

namespace DictService.Helpers
{
   class SyncHelper
    {
       public static void SyncDictService(string connectionString)
        {
            var octopusServerDa = new OctopusServerDataAdapter(connectionString);
            List<OctopusServer> octopusServers = octopusServerDa.GetAll();
           foreach (var octopusServer in octopusServers)
           {
               var octopusDataHelper =
                   new OctopusDataHelper($"Server={octopusServer.Serverip};Database={octopusServer.OctopusDbName};Trusted_Connection=True;");
               PassToDictService(octopusServer.Server_id, connectionString, octopusDataHelper);
           }
        }

       static void PassToDictService(int serverId,string connectionString, OctopusDataHelper helper)
       {
           const string query = @"EXEC SyncDB @Server_id, @Task, @Project, @Module";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query))
            {
                cmd.Connection = conn;
                var param0 = new SqlParameter("@Server_id", SqlDbType.Int);
                var param1 = new SqlParameter("@Task", SqlDbType.Structured);
                var param2 = new SqlParameter("@Project", SqlDbType.Structured);
                var param3 = new SqlParameter("@Module", SqlDbType.Structured);
                param1.TypeName = "dbo.SyncTask";
                param2.TypeName = "dbo.SyncProject";
                param3.TypeName = "dbo.SyncModule";
                param0.Value = serverId;
                param1.Value = helper.OctopusDataSet.Tables["Task"];
                param2.Value = helper.OctopusDataSet.Tables["Project"];
                param3.Value = helper.OctopusDataSet.Tables["Module"];
                cmd.Parameters.Add(param0);
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
