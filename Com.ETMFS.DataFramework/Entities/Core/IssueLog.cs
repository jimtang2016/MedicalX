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
     [Table("T_IssueLog")]
    public partial class IssueLog
    {
         HashSet<AssignedUser> _assignUsers = null;
         public IssueLog()
         {
             _assignUsers = new HashSet<AssignedUser>();
         }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Reason { get; set; }
        public int? DocumentId { get; set; }
        public int? ReviewerId { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ReviewDate { get; set; }
        public string Comments { get; set; }
        [StringLength(200)]
        public string ReviewerName { get; set; }
        public string LogNum { get; set; }
        public string Status { get; set; }
        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }
        public virtual StudyDocument StudyDocument { get; set; }
       
        public virtual Users Reviewer { get; set; }
        public virtual ICollection<AssignedUser> AssignedUsers { get { return _assignUsers; } }
       
    }

     [Table("T_IssueLogAssignUser")]
    public partial class AssignedUser
    {
        [Key]
        public int Id { get; set; }
        public int? AssignUserId { get; set; }
        public int? IssueLogId { get; set; }
        public bool? IsOther { get; set; }
        public virtual Users AssignUser { get; set; }
        public virtual IssueLog IssueLog { get; set; }
    }
}
