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
    
    public partial class expenceTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public expenceTable()
        {
            this.invoiceTable = new HashSet<invoiceTable>();
        }
    
        public int expenceID { get; set; }
        public string expenceCode { get; set; }
        public string expenceDescription { get; set; }
        public Nullable<bool> expenceBool { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoiceTable> invoiceTable { get; set; }
    }
}
