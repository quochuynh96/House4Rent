﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplicationFPTGroup14.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Group14Entities : DbContext
    {
        public Group14Entities()
            : base("name=Group14Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountStatu> AccountStatus { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<HouseForRent> HouseForRents { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Renter> Renters { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Township> Townships { get; set; }
        public virtual DbSet<University> Universities { get; set; }
        public virtual DbSet<Info> Infoes { get; set; }
        public virtual DbSet<Messenger> Messengers { get; set; }
        public virtual DbSet<SearchResult> SearchResults { get; set; }
    }
}
