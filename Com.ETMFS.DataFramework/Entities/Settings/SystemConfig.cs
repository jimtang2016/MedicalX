using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.Entities.Settings
{
    [Table("T_SystemConfig")]
   public partial class SystemConfig
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id{get;set;}
      public string ConfigKey{get;set;}
      public string ConfigXML { get; set; }
    }
}
