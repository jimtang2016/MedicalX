using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;

namespace Com.ETMFS.DataFramework.Entities.History
{
    [Table("T_StudyDocumentHistory")]
    public partial class StudyDocumentHistory
    {
        public int Id { get; set; }
        public int StudyTemplateId { get; set; }
        [StringLength(500)]
        public string DocumentName { get; set; }
        [StringLength(100)]
        public string VersionId { get; set; }
        [StringLength(100)]
        public string Operation { get; set; }
        public string Comments { get; set; }
        public string DocumentType { get; set; }
        public int? ReviewId { get; set; }
        public int? SiteId { get; set; }
        public int UploaderId { get; set; }
        public int? CountryId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }
        [StringLength(100)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }
        [StringLength(100)]
        public string ModifiBy { get; set; }
        public int? DocumentId { get; set; }
       
        
        public StudyDocument StudyDocument { get; set; }
    }
}
