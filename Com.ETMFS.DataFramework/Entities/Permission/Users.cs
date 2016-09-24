namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("T_Users")]
    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            LoginHistory = new HashSet<LoginHistory>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        public string Password { get; set; }

        public int? UserGroupId { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        public int? CompanyId { get; set; }

        public bool? IsMainContact { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

        public virtual Company Company { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoginHistory> LoginHistory { get; set; }

        public virtual UserGroups UserGroups { get; set; }
    }
}
