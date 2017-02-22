﻿using System;
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
        public string Status { get; set; }
       
        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }
        [StringLength(100)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }
        [StringLength(100)]
        public string ModifiBy { get; set; }
        public StudyTemplate StudyTemplate { get; set; }
        public virtual ICollection<StudyDocumentHistory> StudyDocumentHistory { get { return studydocument; } }
        public ICollection<IssueLog> IssueLogs { get { return _issuelogs; } }
    }

   
}
