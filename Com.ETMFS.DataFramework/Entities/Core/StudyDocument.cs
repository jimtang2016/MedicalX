using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using Com.ETMFS.DataFramework.Entities.History;
namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_StudyDocument")]
    public partial class StudyDocument
    {
        ICollection<StudyDocumentHistory> studydocument = null;
        ICollection<IssueLog> _issuelogs = null;
        public StudyDocument()
        {
            studydocument = new List<StudyDocumentHistory>();
            _issuelogs = new List<IssueLog>();
        }

        public int Id { get; set; }
        public int StudyTemplateId { get; set; }
        [StringLength(500)]
        public string DocumentName { get; set; }
        [StringLength(100)]
        public string VersionId { get; set; }
        public string Comments { get; set; }
        public string DocumentType { get; set; }
        public int? ReviewId { get; set; }
        public int UploaderId { get; set; }
        public int? SiteId { get; set; }
        public int? CountryId { get; set; }
        public bool? Active { get; set; }

        public bool? HasIssue { get; set; }
        public string IssueLoges { get; set; }
        public string IssueLogIds { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }
        [StringLength(100)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }
        [StringLength(100)]
        public string ModifiBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? DocumentDate { get; set; }
        public string Status { get; set; }
        public string DocumentLevel { get; set; }
        public string ProtocolNumber { get; set; }
        public string Language { get; set; }
        public bool? IsCountryShared { get; set; }
        public bool? IsSiteShared { get; set; }
        public string SharedCountryIds { get; set; }
        public string SharedCountryNames { get; set; }
        public string SharedSiteIds { get; set; }
        public string SharedSiteNames { get; set; }
        public string TMFType { get; set; }
        public StudyTemplate StudyTemplate { get; set; }

        public virtual ICollection<StudyDocumentHistory> StudyDocumentHistory { get { return studydocument; } }
        public virtual ICollection<IssueLog> IssueLogs { get { return _issuelogs; } }


    }

   
}
