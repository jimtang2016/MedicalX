using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.Entities.Core
{
    [Table("T_Country")]
  public  class Country
    {
        ICollection<TrialRegional> _trialRegional = null;
        public Country()
        {
            _trialRegional = new HashSet<TrialRegional>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }
        [MaxLength(100)]
        public string CountryName { set; get; }
        [MaxLength(100)]
        public string CountryCode { set; get; }
        public bool? Active { set; get; }
        public DateTime? Modified { set; get; }
        public DateTime? Created { set; get; }
        [MaxLength(100)]
        public string ModifiedBy { set; get; }
        [MaxLength(100)]
        public string CreatedBy { set; get; }
        
        public virtual ICollection<TrialRegional> TrialRegional { get { return _trialRegional; } }
    }
}
