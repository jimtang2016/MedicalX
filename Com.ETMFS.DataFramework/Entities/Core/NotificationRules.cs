using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.Entities.Core
{
     [Table("T_Notification")]
  public partial  class NotificationRules
    
    {
         [Key]
         [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
         public int Id { get; set; }
         public int? StudySiteId { get; set; }
         public int? MileStoneId { get; set; }
         public int? StudyCountryId { get; set; }
         public int? StudyId { get; set; }
         public int? AlertType { get; set; }
         public int? TriggerDay { get; set; }
         public int? StudyTMFId { get; set; }
         [Column(TypeName = "datetime2")]
         public DateTime? TriggerOnDate { get; set; }
         public int? AlertRule { get; set; }
         public string RuleField { get; set; }
         public virtual StudyTemplate StudyTemplate { get; set; }
         public bool? Active { get; set; }
         
    }
}
