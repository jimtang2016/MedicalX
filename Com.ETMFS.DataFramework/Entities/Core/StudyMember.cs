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
      
        public string CountryName { get; set; }
       
        public string SiteName { get; set; }
        public virtual Study Study { get; set; }
        public virtual UserGroups UserGroup { get; set; }
        public int? GroupId { get; set; }
    }

    [Table("TrialMemberView")]
    public partial class TrialMemberView
    {
        public int StudyId { get; set; }
        public string StudyNum { get; set; }
        public int Id { get; set; }
        public int StudyMemId { get; set; }
        
        public int? CountryCode { get; set; }
        public string Role { get; set; }
        public string RoleLevel { get; set; }
        public int? SiteId { get; set; }
        public bool? Active { get; set; }
        public string MemberName { get; set; }
        public int MemberId { get; set; }
        public string CountryName { get; set; }
        public string SiteName { get; set; }
    }
}
