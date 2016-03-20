using System;
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
    public interface IFileDownload

    {
        [OperationContract]
        DownloadResponse Download(DownloadRequest info);
    }
    [MessageContract]
    public class DownloadResponse
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName { get; set; }
        [MessageBodyMember(Order = 1)]
        public Stream stream { get; set; }

        //public ResponseDownloadInfo info { get; set; }
    }
    [MessageContract]
    public class DownloadRequest
    {
        [MessageHeader(MustUnderstand = true)]
        public string SenderLogin { get; set; }
        [MessageBodyMember(Order = 1)]
        public int Dictionary_id { get; set; }
    }
}

