using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictService.Helpers;

namespace DictService.Helpers
{
    class OctopusDataHelper
    {
        public SqlConnection connection { get; set; }
        public DataSet OctopusDataSet=new DataSet();
        public OctopusDataHelper(string connection)
        {
            this.connection = new SqlConnection(connection);
            GetOctopusData(OctopusDataSet);
        }
        void GetOctopusData(DataSet ds)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                adapter.SelectCommand= new SqlCommand(Properties.Resources.getTaskInfo, connection);
                adapter.Fill(ds, "Task");
                adapter.SelectCommand = new SqlCommand(Properties.Resources.getProjectInfo, connection);
                adapter.Fill(ds, "Project");
                adapter.SelectCommand = new SqlCommand(Properties.Resources.getModuleInfo, connection);
                adapter.Fill(ds, "Module");
            }
        }
    }
}
