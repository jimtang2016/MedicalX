using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_StudyDocument")]
    public partial class StudyDocument
    {
        public int Id { get; set; }


        public int StudyTemplateId { get; set; }
        [StringLength(500)]
        public string DocumentName { get; set; }
        [StringLength(100)]
        public string VersionId { get; set; }
      
        [StringLength(50)]
        public string CountryCode { get; set; }


        public bool? Active { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

      
        public StudyTemplate StudyTemplate { get; set; }
    }
}
