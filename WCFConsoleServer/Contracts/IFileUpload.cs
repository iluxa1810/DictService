﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WCFConsoleServer.Enums;

namespace WCFConsoleServer.Contracts
{

    [ServiceContract]
    public interface IFileUpload

    {
        [FaultContract(typeof(Exception))]
        [OperationContract]
        UploadResponse Upload(UploadRequest uploadRequest);

    }

    [MessageContract]
    public class UploadResponse
    {
        [MessageHeader(MustUnderstand = true)]
        public bool UploadSucceeded { get; set; }

        public string Error { get; set; }
    }

    [MessageContract]
    public class UploadRequest
    {
        [MessageHeader(MustUnderstand = true)]
        public DictionaryInfo DictInfo { get; set; }
        [MessageBodyMember(Order = 1)]
        public Stream Stream { get; set; }
    }


    [DataContract]
    public class DictionaryInfo
    {
        [DataMember]
        public int? Dictionary_id { get; set; }
        [DataMember]
        public int Category_id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string FriendlyName { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public ActionEnum Action;
        [DataMember]
        public string SenderLogin { get; set; }
    }
}

