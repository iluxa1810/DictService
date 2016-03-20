using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml.Linq;
using WCFConsoleServer.Enums;
using WCFConsoleServer.Models;

namespace WCFConsoleServer.Contracts
{
    [ServiceContract]
    public interface IData
    {
       [FaultContract(typeof(Exception))]
        [OperationContract]
        DictionaryDataPackage GetDictionaryData(string user);
        [OperationContract]
        List<CategoryData> GetCategories();
        [OperationContract]
        void AddCategory(string name, string description);
        [OperationContract]
        void ChangeDictionaryStatus(int dictionary_id, DictionaryStateEnum state);
        [OperationContract]
        void ChangeDictionaryInfo(DictionaryData dictData);
        [OperationContract]
        DictionaryProjectPackage GetProjectsData();
        [OperationContract]
        string GetXmlFormConfig(int server_id, int project_id, int task_id);
        [OperationContract]
        void UpdateXmlFormConfig(int server_id, int project_id, int task_id, string xmlConfig);
        [OperationContract]
        DictionaryOnTaskPackage GetDictionariesOnTask(int server_id, int project_id, int task_id);
    }

    public class DictionaryOnTaskPackage
    {
        [DataMember]
        public List<DictionaryData> dictionaryDatas { get; set; } = new List<DictionaryData>();
        [DataMember]
        public List<CategoryData> Categories { get; set; } = new List<CategoryData>();
        [DataMember]
        public List<DictionaryOnTaskData> DictionaryOnTaskDatas { get; set; } = new List<DictionaryOnTaskData>();
    }
    [DataContract]
    public class DictionaryDataPackage
    {
        [DataMember]
        public List<DictionaryData> dictionaryDatas { get; set; } = new List<DictionaryData>();
        [DataMember]
        public List<CategoryData> Categories { get; set; } = new List<CategoryData>();
        //[DataMember]
        //public List<ActionEnum> ActionEnums { get; set; } = new List<ActionEnum>();
    }
    public class DictionaryProjectPackage
    {
        public List<ProjectData> ProjectDatas { get; set; } = new List<ProjectData>();
        public List<TaskData> TaskDatas { get; set; } = new List<TaskData>();
        public List<OctopusServerData> OctopusServerssDatas { get; set; } = new List<OctopusServerData>();
    }
    public class OctopusServerData
    {
        [DataMember]
        public int Server_id { get; set; }
        [DataMember]
        public string ServerName { get; set; }
    }
    [DataContract]
    public class TaskData
    {
        [DataMember]
        public int Server_id { get; set; }
        [DataMember]
        public int Project_id { get; set; }
        [DataMember]
        public int Task_id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public TaskStateEnum State { get; set; }
    }
    [DataContract]
    public class ProjectData
    {
        [DataMember]
        public int Server_id { get; set; }
        [DataMember]
        public int Project_id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
    [DataContract]
    public class DictionaryData
    {
        [DataMember]
        public int Dictionary_id { get; set; }
        [DataMember]
        public string FriendlyName { get; set; }
        [DataMember]
        public int Category_id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DictionaryStateEnum State { get; set; }
    }
    [DataContract]
    public class CategoryData
    {
        [DataMember]
        public int Category_id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    public class DictionaryOnTaskData
    {
        [DataMember]
        public int Dictionary_id { get; set; }
        [DataMember]
        public int DictionaryOnTask_id { get; set; }
    }
}
