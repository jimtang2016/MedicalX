using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_StudyCountry")]
   public class TrialRegional
    {
        [Key]
        public int Id { set; get; }
      
        public int? CountryId { set; get; }
        public int? StudyId{ set; get; }
        public int? OwnerId { set; get; }
        public virtual Study Study { get; set; }
        public virtual Country Country { get; set; }

        public bool? Active { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }
    }
}
