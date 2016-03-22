using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.ServiceModel;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using System.Xml.XPath;
using Common.DictServiceEnums.DictServiceEnums;
using Common.Helpers;
using Dapper;
using WCFConsoleServer.Contracts;
using WCFConsoleServer.Enums;
using WCFConsoleServer.Models;
using Version = WCFConsoleServer.Models.Version;

namespace WCFConsoleServer.Services
{
    class DictionaryService : IFileUpload, IFileDownload, IData
    {
        readonly string RepoPath = Properties.Settings.Default.DictionaryRepo;
        bool CheckExtension(string fileName, string pattern)
        {
            Console.WriteLine(Path.GetExtension(fileName));
            if (Path.GetExtension(fileName) == pattern)
            {
                return true;
            }
            return false;
        }
        bool CheckPermission(User user, ActionEnum action)
        {
            using (var db = new DictServiceEntities())
            {
                var result = from t1 in db.UserPermissions
                             where t1.User_id == user.User_id && t1.DateDel == null
                             join t2 in db.Permissions on t1.Permission equals t2
                             from t3 in t2.ActionTypes
                             where t3.Action_id == (int)action
                             join t4 in db.ActionTypes on t3 equals t4
                             select new { f1 = t2.Permission_id };
                if (!result.Any())
                    return false;
                else
                    return true;
            }
        }
        List<ActionEnum> GetPermissions(string userLogin)
        {
            using (var db = new DictServiceEntities())
            {
                var result = (from t1 in db.Users
                              where t1.Login == userLogin && t1.DateDel == null
                              join t2 in db.UserPermissions on t1 equals t2.User
                              join t3 in db.Permissions on t2.Permission equals t3
                              from t4 in t3.ActionTypes
                              select new { t4 }).Distinct();
                var list = new List<ActionEnum>();
                foreach (var r in result)
                {
                    list.Add((ActionEnum)r.t4.Action_id);
                }
                return list;
            }
        }
        public UploadResponse Upload(UploadRequest request)
        {
            try
            {
                DictionaryInfo dictInfo = request.DictInfo;
                using (var db = new DictServiceEntities())
                {
                    var user = db.Users.SingleOrDefault(x => x.Login == dictInfo.SenderLogin);
                    if (user == null)
                    {
                        user.Login = request.DictInfo.SenderLogin;
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    //if (!CheckExtension(dictInfo.FileName, ".mdb"))
                    //{
                    //    throw new NotSupportedException();
                    //}
                    if (!CheckPermission(user, dictInfo.Action))
                    {
                        throw new Exception("Нету прав!!!");
                    }
                    switch (dictInfo.Action)
                    {
                        case ActionEnum.AddDict:
                            {
                                AddNewDicionary(request.DictInfo, user, request.Stream);
                                break;
                            }
                        case ActionEnum.EditDict:
                            {
                                RefreshDictionary(request.DictInfo, user, request.Stream);
                                break;
                            }
                    }
                    return new UploadResponse
                    {
                        UploadSucceeded = true
                    };
                }
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return new UploadResponse
                {
                    UploadSucceeded = false,
                    Error = dbEx.ToString()
                };
            }
        }
        string GetUploadPath(int categoryId, string friendlyName, string fileName)
        {
            using (var db = new DictServiceEntities())
            {
                var category = db.DictionaryCategories.Single(x => x.Category_id == categoryId).Name;
                //категория/название словаря/словарь.mdb
                var uploadPath = Path.Combine(new[] { this.RepoPath, category, friendlyName });
                return uploadPath;
            }
        }

        int NewDictFileProcessing(Stream stream, string uploadPath, DictionaryInfo dictInfo)
        {
            var tmpFolder = FileHelper.GetTemporaryDirectory();
            var tmpFile = Path.Combine(tmpFolder, dictInfo.FileName);
            FileHelper.LoadFileFromStream(stream, tmpFile);
            ZipHelper.UnZip(tmpFile);
            var file = Directory.GetFiles(tmpFolder, "*.mdb").Single();
            if (!dictInfo.Dictionary_id.HasValue)
            {
                using (var db = new DictServiceEntities())
                {
                    var dict = new Dictionary()
                    {
                        FileName = dictInfo.FileName,
                        Category_id = dictInfo.Category_id,
                        Description = dictInfo.Description,
                        PathToDict = Path.Combine(uploadPath, dictInfo.FileName),
                        FriendlyName = dictInfo.FriendlyName
                    };
                    db.Dictionaries.Add(dict);
                    db.SaveChanges();
                    AccessHelper.SetDbStamp(file, dict.Dictionary_id);
                    dictInfo.Dictionary_id = dict.Dictionary_id;
                }
            }
            ZipHelper.CreateZipDictionary(file, uploadPath);
            return dictInfo.Dictionary_id.Value;
        }
        void ExistDictFileProcessing(Stream stream, Dictionary dict,UserChangeHistory change)
        {
            var tmpFolder = FileHelper.GetTemporaryDirectory();
            var tmpFile = Path.Combine(tmpFolder, Path.GetFileName(dict.FileName));
            FileHelper.LoadFileFromStream(stream, tmpFile);
            ZipHelper.UnZip(tmpFile);
            var newFile = Directory.GetFiles(tmpFolder, "*.mdb").Single();

            if (AccessHelper.CheckIdentifyInfo(newFile) == dict.Dictionary_id)
            {
                var unZipFoler=ZipHelper.UnZipToTempDir(dict.PathToDict);
                var oldDict= Directory.GetFiles(unZipFoler, "*.mdb").Single();
                var changes = DBComparer.CompareDataBase(newFile, oldDict);
                if (changes.Any())
                {
                    using (var db = new DictServiceEntities())
                    {
                        db.DictionaryChangeHistories.AddRange(changes.Select(x => new DictionaryChangeHistory()
                        {
                            UserHistory_id = change.UserHistory_id,
                            Change_id = (int)x.ChangeType,
                            Dictionary_id = dict.Dictionary_id,
                            TableName = x.TableName,
                            PrimaryKey = x.PrimaryKey,
                            ColumnName = x.ColumnName,
                            OldValue = x.OldValue,
                            NewValue = x.NewValue
                        }));
                        db.SaveChanges();
                    }
                    CreateDictionaryVersion(dict);
                    File.Move(tmpFile, dict.PathToDict);
                }
            }
            else
            {
                throw new Exception("Попытка загрузить неизвестный словарь");
            }
        }
        private void AddNewDicionary(DictionaryInfo dictInfo, User user, Stream stream)
        {
            string uploadPath = GetUploadPath(dictInfo.Category_id, dictInfo.FriendlyName, dictInfo.FileName);
            // FileHelper.LoadFileFromStream(stream, uploadPath);
            var id = NewDictFileProcessing(stream, uploadPath, dictInfo);
            using (var db = new DictServiceEntities())
            {
                var change = new UserChangeHistory()
                {
                    User_id = user.User_id,
                    Dictionary_id = id,
                    Action_id = (int)dictInfo.Action,
                    DateHistory = DateTime.Now
                };
                db.UserChangeHistories.Add(change);
                db.SaveChanges();
            }
        }
        public DownloadResponse Download(DownloadRequest info)
        {
            using (var db = new DictServiceEntities())
            {
                var dict =
                    db.Dictionaries.Single(x => x.Dictionary_id == info.Dictionary_id);
                Stream file = new FileStream(dict.PathToDict, FileMode.Open, FileAccess.Read, FileShare.Read);
                OperationContext clientContext = OperationContext.Current;
                clientContext.OperationCompleted += delegate
                {
                    file.Dispose();
                };
                return new DownloadResponse { stream = file, FileName = dict.FileName };
            }
        }
        Dictionary RefreshDictionary(DictionaryInfo dictInfo, User user, Stream stream)
        {
            using (var db = new DictServiceEntities())
            {
                var dict = db.Dictionaries.Single(x => x.Dictionary_id == dictInfo.Dictionary_id.Value);
                dict.DictionaryState = db.DictionaryStates.Single(x => x.State_id == (int)DictionaryStateEnum.Refreshing);
                db.Entry(dict).State = EntityState.Modified;
                var change = new UserChangeHistory()
                {
                    Dictionary_id = dict.Dictionary_id,
                    Action_id = (int)ActionEnum.EditDict,
                    DateHistory = DateTime.Now,
                    User_id = user.User_id
                };
                db.UserChangeHistories.Add(change);
                db.SaveChanges();
                ExistDictFileProcessing(stream, dict, change);
                dict.DictionaryState = db.DictionaryStates.Single(x => x.State_id == (int)DictionaryStateEnum.Available);
                db.Entry(dict).State = EntityState.Modified;

                db.SaveChanges();
                AddNewQueue(dict.Dictionary_id, ActionEnum.EditDict, change.UserHistory_id);
                return dict;
            }
        }
        void AddNewQueue(int dictId, ActionEnum action, int changeId)
        {
            using (var db = new DictServiceEntities())
            {
                if (!db.DictionaryOnTasks.Any(x => x.Dictionary.Dictionary_id == dictId && x.DateDel == null))
                {
                    return;
                }
                var result = db.DictionaryOnTasks.Where(x => x.Dictionary.Dictionary_id == dictId && x.DateDel == null);
                foreach (var element in result)
                {
                    var queue = new Queue()
                    {
                        Project_id = element.Project_id,
                        Server_id = element.Server_id,
                        Task = element.Task,
                        Action_id = (int)action,
                        UserHistory_id = changeId
                    };
                    db.Queues.Add(queue);
                }
                db.SaveChanges();
            }
        }
        public void CreateDictionaryVersion(Dictionary dict)
        {
            using (var db = new DictServiceEntities())
            {
                string PathToVersions = Path.Combine(Path.GetDirectoryName(dict.PathToDict), "Versions");
                string newFileName = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss_") + Path.GetFileName(dict.PathToDict);
                if (!Directory.Exists(PathToVersions))
                {
                    Directory.CreateDirectory(PathToVersions);
                }
                string dstPath = Path.Combine(PathToVersions, newFileName);
                File.Copy(dict.PathToDict, dstPath);
                File.Delete(dict.PathToDict);
                var version = new Version()
                {
                    Dictionary_id = dict.Dictionary_id,
                    PathToVersion = dstPath,
                    DateAdd = DateTime.Now
                };
                db.Versions.Add(version);
                db.SaveChanges();
            }
        }
        public DictionaryDataPackage GetDictionaryData(string UserLogin)
        {
            using (var db = new DictServiceEntities())
            {
                var pack = new DictionaryDataPackage();
                pack.dictionaryDatas.AddRange(db.Dictionaries.Select(x => new DictionaryData()
                {
                    FriendlyName = x.FriendlyName,
                    Category_id = x.Category_id,
                    Dictionary_id = x.Dictionary_id,
                    Description = x.Description,
                    State = (DictionaryStateEnum)x.State_id
                }));
                pack.Categories.AddRange(db.DictionaryCategories.Select(x => new CategoryData()
                {
                    Category_id = x.Category_id,
                    Name = x.Name
                }));
                //  pack.ActionEnums = GetPermissions(UserLogin);
                return pack;
            }
        }
        public List<CategoryData> GetCategories()
        {
            using (var db = new DictServiceEntities())
            {
                return db.DictionaryCategories.Select(x => new CategoryData { Name = x.Name, Category_id = x.Category_id }).ToList();
            }
        }
        public void AddCategory(string name, string description)
        {
            using (var db = new DictServiceEntities())
            {
                var category = new DictionaryCategory()
                {
                    Name = name,
                    Description = description
                };
                db.DictionaryCategories.Add(category);
                db.SaveChanges();
            }
        }

        public void ChangeDictionaryStatus(int dictionary_id, DictionaryStateEnum state)
        {
            using (var db = new DictServiceEntities())
            {
                var dict = db.Dictionaries.Single(x => x.Dictionary_id == dictionary_id);
                dict.State_id = (int)state;
                db.Entry(dict).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        string TransferDictionary(string oldPath, string categoryName, string friendlyName)
        {
            string newPath = Path.Combine(Properties.Settings.Default.DictionaryRepo, categoryName, friendlyName);
            try
            {
                FileHelper.DirectoryCopy(Path.GetDirectoryName(oldPath), newPath, true);
                FileHelper.DeleteFolder(Path.GetDirectoryName(oldPath));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return newPath;
        }
        public void ChangeDictionaryInfo(DictionaryData dictData)
        {
            using (var db = new DictServiceEntities())
            {//Проверка прав, запись в хистори
                var dict = db.Dictionaries.Single(x => x.Dictionary_id == dictData.Dictionary_id);
                if (dictData.Category_id != dict.Category_id || dictData.FriendlyName != dict.FriendlyName)
                {
                    var categoryName = db.DictionaryCategories.Single(x => x.Category_id == dictData.Category_id).Name;
                    var newPath = TransferDictionary(dict.PathToDict, categoryName, dictData.FriendlyName);
                    var versions = db.Versions.Where(x => x.Dictionary_id == dict.Dictionary_id && x.DateDel == null).ToList();
                    if (versions.Any())
                    {
                        versions.ForEach(x => x.PathToVersion = x.PathToVersion.Replace(Path.GetDirectoryName(x.PathToVersion), newPath));
                    }
                    dict.PathToDict = dict.PathToDict.Replace(Path.GetDirectoryName(dict.PathToDict), newPath);
                    dict.Category_id = dictData.Category_id;
                    dict.FriendlyName = dictData.FriendlyName;
                }
                dict.Description = dictData.Description;
                db.SaveChanges();
            }
        }
        public DictionaryProjectPackage GetProjectsData()
        {
            var pack = new DictionaryProjectPackage();
            using (var db = new DictServiceEntities())
            {
                pack.TaskDatas = db.Tasks.Select(x => new TaskData()
                {
                    Name = x.Name,
                    Project_id = x.Project_id,
                    Server_id = x.Server_id,
                    Task_id = x.Task_id,
                    State = (TaskStateEnum)x.State_id
                }).ToList();
                pack.ProjectDatas =
                    db.Projects.Select(x => new ProjectData()
                    {
                        Server_id = x.Server_id,
                        Project_id = x.Project_id,
                        Name = x.Name
                    }).ToList();
                pack.OctopusServerssDatas =
                    db.OctopusServers.Select(
                        x => new OctopusServerData()
                        {
                            ServerName = x.ServerName,
                            Server_id = x.Server_id
                        }).ToList();
            }
            return pack;
        }
        public string GetXmlFormConfig(int server_id, int project_id, int task_id)
        {
            string connString = string.Empty;
            using (var db = new DictServiceEntities())
            {
                var server = db.OctopusServers.Single(x => x.Server_id == server_id);
                connString =
                    $"Data Source={server.ServerIp};Initial Catalog={server.OctopusDbName};Integrated Security=True";
            }
            using (var conn = new SqlConnection(connString))
            {
                return conn.Query<string>(@"select XMLConfig 
                                            from [Task] 
                                            where project_id=@project_id and task_id=@task_id",
                    new { project_id, task_id }).Single();
            }
        }
        public void UpdateXmlFormConfig(int server_id, int project_id, int task_id, string xmlConfig)
        {

            var reader = new StringReader(xmlConfig);
            XDocument xDoc = XDocument.Load(reader);
            List<int> dictionaryIds;
            dictionaryIds = ApplyChanges(xDoc, server_id, project_id, task_id);
            if (!dictionaryIds.Any())
                return;

            CopyDictionaryToModule(dictionaryIds, GetPathToModule(server_id, project_id, task_id));
            SendConfigToServer(server_id, project_id, task_id, xDoc.ToString());

        }

        void SendConfigToServer(int server_id, int project_id, int task_id, string xmlConfig)
        {
            string connString = string.Empty;
            using (var db = new DictServiceEntities())
            {
                var server = db.OctopusServers.Single(x => x.Server_id == server_id);
                connString =
                    $"Data Source={server.ServerIp};Initial Catalog={server.OctopusDbName};Integrated Security=True";
            }
            using (var conn = new SqlConnection(connString))
            {
                conn.Execute(@"UPDATE [Task]
                                SET XMLConfig=@xmlConfig 
                                where project_id=@project_id and task_id=@task_id",
                    new { xmlConfig, project_id, task_id });
            }
        }

        void CopyDictionaryToModule(List<int> dictionaryIds, string modulePath)
        {
            using (var db = new DictServiceEntities())
            {
                foreach (var id in dictionaryIds)
                {
                    var dict = db.Dictionaries.Single(x => x.Dictionary_id == id);
                    string dstPath = Path.Combine(modulePath, "Apex.zip");
                    if (!Directory.Exists(Path.GetDirectoryName(dstPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(dstPath));
                    }
                    if (File.Exists(dstPath))
                    {
                        //Игнорировать
                    }
                    var tmpFolder = FileHelper.GetTemporaryDirectory();
                    ZipHelper.UnZip(dict.PathToDict, tmpFolder);
                    var file = Directory.GetFiles(tmpFolder, "*.mdb").Single();
                    ZipHelper.AddDictionaryToZip(dstPath, file);
                }
            }
        }
        public DictionaryOnTaskPackage GetDictionariesOnTask(int server_id, int project_id, int task_id)
        {
            using (var db = new DictServiceEntities())
            {
                var result = new DictionaryOnTaskPackage();
                result.dictionaryDatas = (from t1 in db.Dictionaries
                                          where
                                              t1.DictionaryOnTasks.Any(
                                                  x => x.Server_id == server_id && x.Project_id == project_id && x.Task_id == task_id && x.DateDel == null)
                                          select new DictionaryData
                                          {
                                              FriendlyName = t1.FriendlyName,
                                              Category_id = t1.Category_id,
                                              Description = t1.Description,
                                              Dictionary_id = t1.Dictionary_id
                                          }).ToList();
                result.Categories =
                    db.DictionaryCategories.Select(x => new CategoryData() { Category_id = x.Category_id, Name = x.Name })
                        .ToList();
                result.DictionaryOnTaskDatas = db.DictionaryOnTasks.Select(x => new DictionaryOnTaskData()
                {
                    DictionaryOnTask_id = x.DictionaryOnTask_id,
                    Dictionary_id = x.Dictionary_id
                }).ToList();
                return result;
            }
        }

        List<int> ApplyChanges(XDocument xml, int server_id, int project_id, int task_id)
        {
            List<int> dictIds = new List<int>();
            int tmp;
            var extensions =
              xml.XPathSelectElements("*//Extenders/Ext[((contains(@Id,'SimpleDictionary') or contains(@Id,'ImportDictionary')) and @Event)]").Select(x => new
              {
                  DictType = x.Attribute("Id").Value,
                  DictionaryOnTask_id = int.TryParse(x.Attribute("DictionaryOnTask_id").Value, out tmp) ? tmp : default(int?)
                                           ,
                  Dictionary_id = int.Parse(x.Attribute("Dictionary_id").Value),
                  Event = x.Attribute("Event")?.Value ?? "",
                  Ext = x
              }).ToList();
            using (var db = new DictServiceEntities())
            {
                foreach (var ext in extensions)
                {
                    if (ext.Event == "")
                    {
                        ext.Ext.Attribute("Event").Remove();
                        continue;
                    }
                    if (ext.Event == "Add")
                    {
                        var dictOnTask = new DictionaryOnTask()
                        {
                            Dictionary_id = ext.Dictionary_id,
                            Server_id = server_id,
                            Project_id = project_id,
                            Task_id = task_id,
                            DateAdded = DateTime.Now,
                            DateSync = DateTime.Now
                        };
                        db.DictionaryOnTasks.Add(dictOnTask);
                        db.SaveChanges();
                        ext.Ext.Attribute("DictionaryOnTask_id").Value = dictOnTask.DictionaryOnTask_id.ToString();
                        ProcessingConnectionString(ext.Ext, ext.DictType, server_id, project_id, task_id, ext.Dictionary_id);
                        dictIds.Add(ext.Dictionary_id);
                        continue;
                    }
                    if (ext.Event == "Remove")
                    {
                        var dictOnTask = db.DictionaryOnTasks.Single(x => x.DictionaryOnTask_id == ext.Dictionary_id);
                        dictOnTask.DateDel = DateTime.Now;
                        ext.Ext.Remove();
                        continue;
                    }
                    if (ext.Event == "Change")
                    {
                        var dictOnTask = db.DictionaryOnTasks.Single(x => x.DictionaryOnTask_id == ext.Dictionary_id);
                        dictOnTask.Dictionary_id = ext.Dictionary_id;
                        ProcessingConnectionString(ext.Ext, ext.DictType, server_id, project_id, task_id, ext.Dictionary_id);
                        ext.Ext.Attribute("Event").Remove();
                        dictIds.Add(ext.Dictionary_id);
                        continue;
                    }
                }
            }
            return dictIds;
        }
        void ProcessingConnectionString(XElement xElement, string dictType, int server_id, int project_id, int task_id, int dictionary_id)
        {
            if (dictType.Contains("Import"))
            {
                xElement.Attribute("ImportDbConnectionString").Value =
                    GetDictionaryConnectionString(server_id, project_id, task_id, Convert.ToInt32(dictionary_id));
            }
            else if (dictType.Contains("Simple"))
            {
                xElement.Attribute("ConnectionString").Value =
                    GetDictionaryConnectionString(server_id, project_id, task_id, Convert.ToInt32(dictionary_id));
            }
        }
        string GetPathToModule(int server_id, int project_id, int task_id)
        {
            using (var db = new DictServiceEntities())
            {
                var result = (from t in db.OctopusServers
                              where t.Server_id == server_id
                              join t2 in db.Projects on t.Server_id equals t2.Server_id
                              where t2.Project_id == project_id
                              join t3 in db.Tasks on new { t2.Project_id, t2.Server_id } equals new { t3.Project_id, t3.Server_id }
                              join t4 in db.Modules on new { t3.Server_id, t3.Module_id } equals new { t4.Server_id, t4.Module_id }
                              where t3.Task_id == task_id
                              select new { t.ClientToolsPath, t4.FilePath }).First();
                return Path.Combine(result.ClientToolsPath, Path.GetDirectoryName(result.FilePath));
            }
        }
        string GetDictionaryConnectionString(int server_id, int project_id, int task_id, int dictonary_id)
        {
            using (var db = new DictServiceEntities())
            {
                var modulePath = GetPathToModule(server_id, project_id, task_id);
                var fullPath = Path.Combine(modulePath,
                    db.Dictionaries.Single(x => x.Dictionary_id == dictonary_id).FileName);
                var connString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={fullPath};Persist Security Info=False"; ;
                return connString;
            }
        }
    }
}
