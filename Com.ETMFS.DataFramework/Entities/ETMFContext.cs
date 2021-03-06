namespace Com.ETMFS.DataFramework.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Com.ETMFS.DataFramework.Entities.Permission;
    using Com.ETMFS.DataFramework.Entities.Core;
    using Com.ETMFS.DataFramework.Entities.History;
    using Com.ETMFS.DataFramework.View;
    using Com.ETMFS.DataFramework.Entities.Settings;
    public partial class ETMFContext : DbContext 
    {
        public ETMFContext()
            : base("name=ETMFContext")
        {
        }

        public virtual DbSet<MileStone> MileStone { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<IssueLog> IssueLog { get; set; }
        public virtual DbSet<AssignedUser> AssignedUser { get; set; }
        public virtual DbSet<FunctionGroup> FunctionGroup { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<LoginHistory> LoginHistory { get; set; }
        public virtual DbSet<OptionList> OptionList { get; set; }
        public virtual DbSet<UserGroups> UserGroups { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<TrialRegional> TrialRegional { get; set; }
        public virtual DbSet<StudyListView> StudyListView { get; set; }
        public virtual DbSet<StudyDocument> StudyDocument { get; set; }
        public virtual DbSet<StudyDocumentHistory> StudyDocumentHistory { get; set; }
        public virtual DbSet<DocumentView> DocumentView { get; set; }
        public virtual DbSet<TrialMemberView> TrialMemberView { get; set; }
        public virtual DbSet<NotifyReminderView> NotifyReminderView { get; set; }
        public virtual DbSet<NotificationRules> NotificationRules { get; set; }
        public virtual DbSet<SystemConfig> SystemConfig { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Company)
                .HasForeignKey(e => e.CompanyId);

            modelBuilder.Entity<StudyDocument>()
                .HasMany(e => e.StudyDocumentHistory)
                .WithOptional(e => e.StudyDocument)
                .HasForeignKey(e => e.DocumentId);

            modelBuilder.Entity<Functions>()
                .HasMany(e => e.FunctionGroup)
                .WithOptional(e => e.Functions)
                .HasForeignKey(e => e.FunctionId);

            modelBuilder.Entity<UserGroups>()
                .HasMany(e => e.FunctionGroup)
                .WithOptional(e => e.T_UserGroups)
                .HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<StudySite>()
                .HasMany(e => e.MileStones)
                .WithOptional(e => e.StudySite)
                .HasForeignKey(e => e.StudySiteId);

            modelBuilder.Entity<TrialRegional>()
             .HasMany(e => e.MileStones)
             .WithOptional(e => e.StudyCountry)
             .HasForeignKey(e => e.StudyCountryId);

            modelBuilder.Entity<IssueLog>().HasMany(o => o.AssignedUsers).WithOptional(o => o.IssueLog).HasForeignKey(f => f.IssueLogId);
            modelBuilder.Entity<Users>().HasMany(o => o.AssignedUsers).WithOptional(o => o.AssignUser).HasForeignKey(f => f.AssignUserId);

            modelBuilder.Entity<StudyDocument>().HasMany(o => o.IssueLogs).WithOptional(o => o.StudyDocument).HasForeignKey(o => o.DocumentId);
            modelBuilder.Entity<Users>().HasMany(o => o.IssueLogs).WithOptional(o => o.Reviewer);

            modelBuilder.Entity<UserGroups>().HasMany(b => b.Users)
    .WithMany(c => c.UserGroups).Map(o =>
    {
        o.MapLeftKey("GroupId");
        o.MapRightKey("UserId");
        o.ToTable("T_UserMapping");
    });



            modelBuilder.Entity<Users>()
                .HasMany(e => e.LoginHistory)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Study>().HasMany(o => o.TrialRegional).WithOptional(e => e.Study).HasForeignKey(e => e.StudyId);
            modelBuilder.Entity<Study>().HasMany(o => o.MileStones).WithOptional(e => e.Study).HasForeignKey(e => e.StudyId);
            modelBuilder.Entity<Country>().HasMany(o => o.TrialRegional).WithOptional(e => e.Country).HasForeignKey(e => e.CountryId);

            modelBuilder.Entity<Study>().HasMany(o => o.StudySite).WithOptional(e => e.Study).HasForeignKey(e => e.StudyId);
            modelBuilder.Entity<Site>().HasMany(o => o.StudySite).WithOptional(e => e.Site).HasForeignKey(e => e.SiteId);

            modelBuilder.Entity<Study>().HasMany(o => o.StudyTemplate).WithRequired(e => e.Study).HasForeignKey(e => e.StudyId);
            modelBuilder.Entity<TMFTemplate>().HasMany(o => o.StudyTemplate).WithRequired(e => e.TMFTemplate).HasForeignKey(e => e.TemplateId);

            modelBuilder.Entity<Study>().HasMany(o => o.StudyMember).WithRequired(e => e.Study).HasForeignKey(e => e.StudyId);

            modelBuilder.Entity<UserGroups>().HasMany(o => o.StudyMember).WithOptional(e => e.UserGroup).HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<StudyTemplate>().HasMany(o => o.StudyDocument).WithRequired(e => e.StudyTemplate).HasForeignKey(e => e.StudyTemplateId);

            modelBuilder.Entity<StudyTemplate>().HasMany(o => o.NotificationRules).WithOptional(e => e.StudyTemplate).HasForeignKey(e => e.StudyTMFId);

            modelBuilder.Entity<StudyTemplate>().HasMany(o => o.TemplateOutcluding).WithRequired(e => e.StudyTemplate).HasForeignKey(e => e.StudyTemplateId);

        }
    }
}
