//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bes.Models.BesEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class FileTables
    {
        public int fileID { get; set; }
        public int store_id { get; set; }
        public int user_id { get; set; }
        public string fileURL { get; set; }
        public string fileName { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    
        public virtual storeTable storeTable { get; set; }
        public virtual userTable userTable { get; set; }
    }
}
