//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace A108222027_finalProject.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MacdonaldEntities : DbContext
    {
        public MacdonaldEntities()
            : base("name=MacdonaldEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<tAccount> tAccount { get; set; }
        public virtual DbSet<tItem> tItem { get; set; }
        public virtual DbSet<tdiscount> tdiscount { get; set; }
    }
}
