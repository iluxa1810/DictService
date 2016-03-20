using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
   static class DBComparer
    {

        //static void Main(string[] args)
        //{
        //    var connStr =
        //        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\iluxa1810\Documents\visual studio 2015\Projects\BDComparer\BDComparer\BDForTest\new.mdb;Persist Security Info=False;";
        //    var connStr2 = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\iluxa1810\Documents\visual studio 2015\Projects\BDComparer\BDComparer\BDForTest\old.mdb;Persist Security Info=False;";
        //    CompareDataBase(connStr, connStr2);
        //}

        public enum ChangeType
        {
            AddTable, RemoveTable, AddColumn, RemoveColumn, EditValue, RemoveRow, AddRow, ChangeKey, ChangeColumnType
        }
        public class Change
        {
            public string TableName { get; set; }
            public ChangeType ChangeType { get; set; }
            public string PrimaryKey { get; set; }
            public string ColumnName { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }


        public static List<Change> CompareDataBase(string newConnStr, string oldConnStr)
        {
            List<DataTable> newDataTables;
            List<DataTable> oldDataTables;
            try
            {
                newDataTables = GetDataTables(newConnStr);
                oldDataTables = GetDataTables(oldConnStr);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при получении данных",ex);
            }
            List<Change> changes = new List<Change>();
            changes.AddRange(GetAddedTables(newDataTables, oldDataTables));
            changes.AddRange(GetRemovedTables(newDataTables, oldDataTables));
           
            foreach (var tableName in GetEqualTableNames(newDataTables, oldDataTables))
            {
                var newTable = newDataTables.Single(x => x.TableName == tableName);
                var oldTable = oldDataTables.Single(x => x.TableName == tableName);
                CompareStructure(newTable, oldTable, changes);
                if (ComparePrimaryKeys(newTable, oldTable, changes))
                {
                    CompareData(newTable, oldTable, changes);
                }
            }
            //foreach (var change in changes)
            //{
            //    Console.WriteLine("Table Name: {0}; FieldName:{1}; ChangeType:{2}; PrimaryKey {3} ;New: {4} ->Old :{5}", change.TableName, change.ColumnName, change.ChangeType, change.PrimaryKey, change.NewValue, change.OldValue);
            //}
            return changes;
        }

        static bool ComparePrimaryKeys(DataTable newTable, DataTable oldTable, List<Change> changes)
        {
            if (!newTable.PrimaryKey.Any() && oldTable.PrimaryKey.Any())
            {
                throw new KeyNotFoundException("У новой таблицы отсутствуют ключи!");
            }
            else if (!newTable.PrimaryKey.Any() && !oldTable.PrimaryKey.Any())
            {
                return false;
            }
            if (
                newTable.PrimaryKey.Select(x => x.ColumnName)
                    .ToArray()
                    .SequenceEqual(oldTable.PrimaryKey.Select(x => x.ColumnName).ToArray()))
            {
                return true;
            }
            var newKeys = String.Join(";", newTable.PrimaryKey.Select(x => x.ColumnName));
            var oldKeys = String.Join(";", oldTable.PrimaryKey.Select(x => x.ColumnName));
            changes.Add(new Change()
            {
                TableName = newTable.TableName,
                ChangeType = ChangeType.ChangeKey,
                OldValue = oldKeys,
                NewValue = newKeys
            });
            return false;
        }
        static List<Change> GetAddedTables(List<DataTable> newDataTables, List<DataTable> oldDataTables)
        {
            return newDataTables.Where(x => oldDataTables.All(y => y.TableName != x.TableName)).Select(x => new Change()
            {
                TableName = x.TableName,
                ChangeType = ChangeType.AddTable
            }).ToList();
        }
        static List<Change> GetRemovedTables(List<DataTable> newDataTables, List<DataTable> oldDataTables)
        {
            return oldDataTables.Where(x => newDataTables.All(y => y.TableName != x.TableName)).Select(x => new Change()
            {
                TableName = x.TableName,
                ChangeType = ChangeType.RemoveTable
            }).ToList();
        }

        static List<string> GetEqualTableNames(List<DataTable> newDataTables, List<DataTable> oldDataTables)
        {
            return oldDataTables.Where(x => newDataTables.Any(y => y.TableName == x.TableName)).Select(x => x.TableName).ToList();
        }
        static void CompareData(DataTable newDT, DataTable oldDT, List<Change> changes)
        {
            DataSet ds = new DataSet("Cmp");
            newDT.TableName = newDT.TableName + ".new.";
            oldDT.TableName = oldDT.TableName + ".old.";
            ds.Tables.AddRange(new[] { newDT, oldDT });
            DataRelation drel = new DataRelation("rel", newDT.PrimaryKey, oldDT.PrimaryKey, false);
            ds.Relations.Add(drel);
            var columnsList = GetEqualColumns(newDT, oldDT);
            changes.AddRange(GetEditValues(newDT, columnsList));
            changes.AddRange(GetAddedValues(newDT, columnsList));
            changes.AddRange(GetRemoveValues(oldDT, columnsList));
        }

        static void CompareStructure(DataTable newDT, DataTable oldDT, List<Change> changes)
        {
            changes.AddRange(GetAddedColumns(newDT, oldDT));
            changes.AddRange(GetRemovedColumns(newDT, oldDT));
            changes.AddRange(GetColumnTypeChanges(newDT, oldDT));
        }

        static List<Change> GetColumnTypeChanges(DataTable newDT, DataTable oldDT)
        {
            var changes = new List<Change>();
            foreach (var columnName in GetEqualColumns(newDT, oldDT))
            {
                if (newDT.Columns[columnName].DataType != oldDT.Columns[columnName].DataType)
                {
                    changes.Add(new Change()
                    {
                        TableName = newDT.TableName,
                        ChangeType = ChangeType.ChangeColumnType,
                        ColumnName = columnName,
                        OldValue = oldDT.Columns[columnName].DataType.FullName,
                        NewValue = newDT.Columns[columnName].DataType.FullName
                    });
                }
            }
            return changes;
        }
        static List<Change> GetEditValues(DataTable newDT, List<string> columnsList)
        {
            var changeList = new List<Change>();
            var primaryKey = newDT.PrimaryKey[0];
            var tableName = newDT.TableName.Replace(".new.", "");
            foreach (var columnName in columnsList)
            {
                changeList.AddRange(newDT.AsEnumerable()
                    .Where(
                        x =>
                            x.GetChildRows("rel").Length > 0 &&
                            x[columnName].ToString() != x.GetChildRows("rel")[0][columnName].ToString())
                    .Select(
                        x => new Change()
                        {
                            TableName = tableName,
                            ChangeType = ChangeType.EditValue,
                            PrimaryKey = x[primaryKey].ToString(),
                            ColumnName = columnName,
                            OldValue = x.GetChildRows("rel")[0][columnName].ToString(),
                            NewValue = x[columnName].ToString()
                        }));
            }
            return changeList;
        }
        static List<Change> GetRemoveValues(DataTable oldDT, List<string> columnsList)
        {
            var changeList = new List<Change>();
            var primaryKey = oldDT.PrimaryKey[0];
            var tableName = oldDT.TableName.Replace(".old.", "");
            foreach (var columnName in columnsList)
            {
                changeList.AddRange(oldDT.AsEnumerable()
                  .Where(
                      x =>
                          x.GetParentRows("rel").Length == 0)
                  .Select(
                      x => new Change()
                      {
                          TableName = tableName,
                          ChangeType = ChangeType.RemoveRow,
                          PrimaryKey = x[primaryKey].ToString(),
                          ColumnName = columnName,
                          OldValue = x[columnName].ToString(),
                          NewValue = null
                      }));
            }
            return changeList;
        }
        static List<Change> GetAddedValues(DataTable newDT, List<string> columnsList)
        {
            var changeList = new List<Change>();
            var primaryKey = newDT.PrimaryKey[0];
            var tableName = newDT.TableName.Replace(".new.", "");
            foreach (var columnName in columnsList)
            {
                changeList.AddRange(newDT.AsEnumerable()
                   .Where(
                       x =>
                           x.GetChildRows("rel").Length == 0)
                   .Select(
                       x => new Change()
                       {
                           TableName = tableName,
                           ChangeType = ChangeType.AddRow,
                           PrimaryKey = x[primaryKey].ToString(),
                           ColumnName = columnName,
                           OldValue = null,
                           NewValue = x[columnName].ToString()
                       }));
            }
            return changeList;
        }
        static List<Change> GetAddedColumns(DataTable newDT, DataTable oldDT)
        {
            var tableName = newDT.TableName.Replace(".new.", "");
            return newDT.Columns.Cast<DataColumn>()
                .Where(x => oldDT.Columns.Cast<DataColumn>().All(y => y.ColumnName != x.ColumnName)).Select(x => new Change()
                {
                    TableName = tableName,
                    ChangeType = ChangeType.AddColumn,
                    ColumnName = x.ColumnName
                }
                ).ToList();
        }

        static List<Change> GetRemovedColumns(DataTable newDT, DataTable oldDT)
        {
            var tableName = newDT.TableName.Replace(".new.", "");
            return oldDT.Columns.Cast<DataColumn>()
              .Where(x => newDT.Columns.Cast<DataColumn>().All(y => y.ColumnName != x.ColumnName)).Select(x => new Change()
              {
                  TableName = tableName,
                  ChangeType = ChangeType.RemoveColumn,
                  ColumnName = x.ColumnName
              }
              ).ToList();
        }
        static List<string> GetEqualColumns(DataTable newDT, DataTable oldDT)
        {
            return oldDT.Columns.Cast<DataColumn>()
                .Where(x => newDT.Columns.Cast<DataColumn>().Any(y => y.ColumnName == x.ColumnName))
                .Select(x => x.ColumnName).ToList();
        }

        static List<DataTable> GetDataTables(string connStr)
        {
            var dtArray = new List<DataTable>();
            using (var conn = new OleDbConnection(connStr))
            {
                conn.Open();
                string[] restrictions = new string[4];
                restrictions[3] = "Table";
                DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions);
                foreach (DataRow table in tables.Rows)
                {
                    var cmd = new OleDbCommand($"select * from [{table[2]}]", conn);
                    using (var da = new OleDbDataAdapter(cmd.CommandText, conn))
                    {
                        var dt = new DataTable();
                        da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        da.Fill(dt);
                        dtArray.Add(dt);
                    }
                }
            }
            return dtArray;
        }
    }
}


