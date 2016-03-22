using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.InteropServices;
using ADOX;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Access;
using Microsoft.Office.Interop.Access.Dao;

namespace Common.Helpers
{
    public class AccessHelper
    {
       public static void CheckExistPrimaryKey(string connStr)
        {
            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string[] restrictions = new string[4];
                restrictions[3] = "Table";
                DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
                foreach (DataRow table in tables.Rows)
                {
                    if (table[2].ToString() == "_DbStamp"|| table[2].ToString().Contains("~"))
                    {
                        continue;
                    }
                    var schema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys,
                        new[] {null, null, table[2]});
                    if (!schema.AsEnumerable().Any())
                    {
                        throw new KeyNotFoundException($"На таблице {table[2]} отсутсвует первичный ключ!");
                    }
                }
            }
        }
        public static string CreateMdbConnectionString(string filepath)
        {
            var builder2 = new OleDbConnectionStringBuilder();
            builder2.Provider = "Microsoft.ACE.OLEDB.12.0";
            builder2.DataSource = filepath;
            OleDbConnectionStringBuilder builder = builder2;
            return builder.ConnectionString;
        }
        public static int CheckIdentifyInfo(string dbfilePath)
        {
            const string sql = @"SELECT * FROM  [_DbStamp]"; // Скрытая таблица, создается при нарезке 
            int dictionary_id=0;
            var dt = new DataTable();
            try
            {
                using (var conn = new OleDbConnection(CreateMdbConnectionString(dbfilePath)))
                {
                    conn.Open();
                    using (var cmd = new OleDbCommand(sql, conn))
                    {
                        using (var da = new OleDbDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                    conn.Close();
                }
                DataRow dr = dt.Select("Key='Dictionary_id'").SingleOrDefault();
                if (dr != null) dictionary_id = Convert.ToInt32(dr["Val"].ToString());
            }
            catch (Exception ex)
            {
              throw new Exception("Неопознанный словарь!");
            }
            return dictionary_id;
        }

        public static void SetDbStampADOX(string fileName, int Dictionary_id)
        {
            Catalog cat = new Catalog();
            object ret;
            ADODB.Connection conn = new ADODB.Connection();
            conn.ConnectionString = CreateMdbConnectionString(fileName);
            conn.Open();
            cat.ActiveConnection = conn;
            conn.Execute(@"CREATE TABLE [_DbStamp] ([Key] TEXT(255) PRIMARY KEY, [Val] TEXT(255));", out ret, 0);
            conn.Execute($"INSERT INTO [_DbStamp] ([Key], [Val]) VALUES ('Dictionary_id', '{Dictionary_id}');", out ret, 0);
            Table tbl = cat.Tables["_DbStamp"];
            tbl.Properties["Jet OLEDB:Table Hidden In Access"].Value = true;
            conn.Close();
        }
        public static void SetDbStamp(string fileName, int Dictionary_id)
        {
            var appClass = new Application();

            appClass.Visible = false;
            appClass.Screen.Application.Visible = false;
            appClass.Screen.Application.UserControl = false;
            appClass.UserControl = false;
            appClass.AutomationSecurity = MsoAutomationSecurity.msoAutomationSecurityLow;
            try
            {
                appClass.OpenCurrentDatabase(fileName, false, "");
                try
                {
                    appClass.CurrentDb()
                        .Execute(@"DROP TABLE [_DbStamp];");
                }
                catch (Exception)
                {
                }
                appClass.CurrentDb()
                    .Execute(@"CREATE TABLE [_DbStamp] ([Key] TEXT(255) PRIMARY KEY, [Val] TEXT(255));");
                appClass.CurrentDb().TableDefs["_DbStamp"].Attributes = (int)TableDefAttributeEnum.dbHiddenObject;
                appClass.CurrentDb()
                    .Execute($@"INSERT INTO [_DbStamp] ([Key], [Val]) VALUES ('Dictionary_id', '{Dictionary_id}');");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                appClass.Quit(AcQuitOption.acQuitSaveAll);
                NAR(appClass);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.GC.Collect();

            }
        }
        public static void CompactAndRepair(string fileName)
        {
            var appClass = new Application();
            appClass.Visible = false;
            appClass.Screen.Application.Visible = false;
            appClass.Screen.Application.UserControl = false;
            appClass.UserControl = false;
            appClass.AutomationSecurity = MsoAutomationSecurity.msoAutomationSecurityLow;

            string tempFile = Path.Combine(Path.GetTempPath(),
                              Path.GetRandomFileName() + Path.GetExtension(fileName));
            try
            {
                appClass.CompactRepair(fileName, tempFile, false);
                var temp = new FileInfo(tempFile);
                temp.CopyTo(fileName, true);
                temp.Delete();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                appClass.Quit(AcQuitOption.acQuitSaveNone);
                NAR(appClass);
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                System.GC.Collect();
            }
        }
        private static void NAR(object o)
        {
            try
            {
                Marshal.ReleaseComObject(o);
                o = null;
            }
            catch
            {
                o = null;
            }
            finally
            {
            }
        }

    }
}