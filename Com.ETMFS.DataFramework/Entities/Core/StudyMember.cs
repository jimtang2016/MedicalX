using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Permission;

namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_StudyMember")]
    public partial class StudyMember
    {
        public int Id { get; set; }

        public int StudyId { get; set; }
        public int? SiteId { get; set; }
        public int MemberId { get; set; }
        [StringLength(100)]
        public string Role { get; set; }
        [StringLength(100)]
        public string RoleLevel { get; set; }
       
        public int? CountryCode { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }
        [NotMapped]
        public string MemberName { get; set; }
         [NotMapped]
        public string CountryName { get; set; }
         [NotMapped]
        public string SiteName { get; set; }
        public virtual Study Study { get; set; }
        public virtual Users User { get; set; }
        [NotMapped]
        public int? OwnerId { get; set; }
    }
}
