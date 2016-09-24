namespace Com.ETMFS.DataFramework.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Com.ETMFS.DataFramework.Entities.Permission;
    public partial class ETMFContext : DbContext 
    {
        public ETMFContext()
            : base("name=ETMFContext")
        {
        }

        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<FunctionGroup> FunctionGroup { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<LoginHistory> LoginHistory { get; set; }
        public virtual DbSet<OptionList> OptionList { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<Functions>()
                .HasMany(e => e.FunctionGroup)
                .WithOptional(e => e.Functions)
                .HasForeignKey(e => e.FunctionId);

            modelBuilder.Entity<UserGroups>()
                .HasMany(e => e.FunctionGroup)
                .WithOptional(e => e.T_UserGroups)
                .HasForeignKey(e => e.GroupId);


            modelBuilder.Entity<UserGroups>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.UserGroups)
                .HasForeignKey(e => e.UserGroupId);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.LoginHistory)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
