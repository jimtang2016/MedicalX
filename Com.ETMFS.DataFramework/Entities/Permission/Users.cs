namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Com.ETMFS.DataFramework.Entities.Core;

    [Table("T_Users")]
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        ICollection<UserGroups> _userGroups = null;
        ICollection<IssueLog> _issueLogs = null;
        ICollection<AssignedUser> _assignedUsers = null;
        public Users()
        {
            LoginHistory = new HashSet<LoginHistory>();
            _userGroups = new HashSet<UserGroups>();
            _issueLogs = new HashSet<IssueLog>();
            _assignedUsers = new HashSet<AssignedUser>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public string Password { get; set; }

        
        [StringLength(100)]
        public string Country { get; set; }

        public int? CompanyId { get; set; }

        public bool? IsMainContact { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [StringLength(100)]
        public string Email { get; set; }
        
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoginHistory> LoginHistory { get; set; }

        public virtual ICollection<UserGroups> UserGroups { get { return _userGroups; } }
  

        public virtual ICollection<IssueLog> IssueLogs { get { return _issueLogs; } }
        public virtual ICollection<AssignedUser> AssignedUsers { get { return _assignedUsers; } }
    }
}
