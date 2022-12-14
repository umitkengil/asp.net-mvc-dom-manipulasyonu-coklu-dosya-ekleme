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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class BESEntities : DbContext
    {
        public BESEntities()
            : base("name=BESEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<bankTable> bankTable { get; set; }
        public virtual DbSet<expenceTable> expenceTable { get; set; }
        public virtual DbSet<FileTables> FileTables { get; set; }
        public virtual DbSet<invoiceLogTable> invoiceLogTable { get; set; }
        public virtual DbSet<invoiceTable> invoiceTable { get; set; }
        public virtual DbSet<odemeTuruTablosu> odemeTuruTablosu { get; set; }
        public virtual DbSet<resfundTable> resfundTable { get; set; }
        public virtual DbSet<resfundTypeTable> resfundTypeTable { get; set; }
        public virtual DbSet<roleTable> roleTable { get; set; }
        public virtual DbSet<storeTable> storeTable { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<userTable> userTable { get; set; }
        public virtual DbSet<vwInvoiceLog> vwInvoiceLog { get; set; }
    
        public virtual ObjectResult<raporHazirlamaSP_Result> raporHazirlamaSP(Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate)
        {
            var startdateParameter = startdate.HasValue ?
                new ObjectParameter("startdate", startdate) :
                new ObjectParameter("startdate", typeof(System.DateTime));
    
            var enddateParameter = enddate.HasValue ?
                new ObjectParameter("enddate", enddate) :
                new ObjectParameter("enddate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<raporHazirlamaSP_Result>("raporHazirlamaSP", startdateParameter, enddateParameter);
        }
    
        public virtual ObjectResult<masrafRaporHazirlamaSP_Result> masrafRaporHazirlamaSP(Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate)
        {
            var startdateParameter = startdate.HasValue ?
                new ObjectParameter("startdate", startdate) :
                new ObjectParameter("startdate", typeof(System.DateTime));
    
            var enddateParameter = enddate.HasValue ?
                new ObjectParameter("enddate", enddate) :
                new ObjectParameter("enddate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<masrafRaporHazirlamaSP_Result>("masrafRaporHazirlamaSP", startdateParameter, enddateParameter);
        }
    
        public virtual ObjectResult<ozetMasrafRaporHazirlamaSP_Result> ozetMasrafRaporHazirlamaSP(Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate, Nullable<int> store_id)
        {
            var startdateParameter = startdate.HasValue ?
                new ObjectParameter("startdate", startdate) :
                new ObjectParameter("startdate", typeof(System.DateTime));
    
            var enddateParameter = enddate.HasValue ?
                new ObjectParameter("enddate", enddate) :
                new ObjectParameter("enddate", typeof(System.DateTime));
    
            var store_idParameter = store_id.HasValue ?
                new ObjectParameter("store_id", store_id) :
                new ObjectParameter("store_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ozetMasrafRaporHazirlamaSP_Result>("ozetMasrafRaporHazirlamaSP", startdateParameter, enddateParameter, store_idParameter);
        }
    }
}
