namespace Com.ETMFS.DataFramework.Entities.Permission
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using Com.ETMFS.DataFramework.Entities.Core;

    [Table("T_UserGroups")]
    public partial class UserGroups
    {

        ICollection<StudyMember> _studymember = null;
      
        public UserGroups()
        {
            FunctionGroup = new HashSet<FunctionGroup>();
            Users = new HashSet<Users>();
            _studymember = new HashSet<StudyMember>();
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string GroupName { get; set; }

        public string Description { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FunctionGroup> FunctionGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Users> Users { get; set; }
        public virtual ICollection<StudyMember> StudyMember { get { return _studymember; } }
    }
}
