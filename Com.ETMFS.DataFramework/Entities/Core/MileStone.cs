using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_MileStone")]
  public partial  class MileStone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? PlanStartDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? PlanEndDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ActualStartDate { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? ActualEndDate { get; set; }
        public int? StudySiteId { get; set; }
        public int? StudyCountryId { get; set; }
        public int? StudyId { get; set; }
        public virtual TrialRegional StudyCountry { get; set; }
        public virtual StudySite StudySite { get; set; }
        public virtual Study Study { get; set; }
    }
}
