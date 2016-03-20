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
    
    public partial class Task
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Task()
        {
            this.DictionaryOnTasks = new HashSet<DictionaryOnTask>();
            this.Queues = new HashSet<Queue>();
        }
    
        public int Server_id { get; set; }
        public int Project_id { get; set; }
        public int Task_id { get; set; }
        public int Module_id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> DateDel { get; set; }
        public int State_id { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DictionaryOnTask> DictionaryOnTasks { get; set; }
        public virtual Module Module { get; set; }
        public virtual Project Project { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Queue> Queues { get; set; }
    }
}
