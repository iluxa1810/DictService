//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFConsoleServer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Queue
    {
        public int Queue_id { get; set; }
        public int Server_id { get; set; }
        public int Project_id { get; set; }
        public int Task_id { get; set; }
        public int Dictionary_id { get; set; }
        public int Action_id { get; set; }
        public string Error { get; set; }
        public Nullable<int> UserHistory_id { get; set; }
    
        public virtual ActionType ActionType { get; set; }
        public virtual Dictionary Dictionary { get; set; }
        public virtual UserChangeHistory UserChangeHistory { get; set; }
        public virtual Task Task { get; set; }
    }
}