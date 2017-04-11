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
    [Table("T_Study")]
    public partial class Study
    {
        ICollection<StudySite> _studysite = null;
        ICollection<StudyTemplate> _studytemplate = null;
        ICollection<StudyMember> _studymember = null;
        ICollection<TrialRegional> _trialRegional = null;
        public Study(){
            _studysite = new HashSet<StudySite>();
            _studytemplate = new HashSet<StudyTemplate>();
            _studymember = new HashSet<StudyMember>();
            _trialRegional = new HashSet<TrialRegional>();
    }
        public int Id { set; get; }
        [MaxLength(100)]
        public string StudyNum { set; get; }
        [MaxLength(500)]
        public string ShortTitle { set; get; }
        [MaxLength(100)]
        public string Status { set; get; }
        public bool? Active { get; set; }
      
        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }

        [StringLength(100)]
        public string CreateBy { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }

        [StringLength(100)]
        public string ModifiBy { get; set; }

        public virtual ICollection<StudySite> StudySite { get { return _studysite; } }
        public virtual ICollection<StudyTemplate> StudyTemplate { get { return _studytemplate; } }
        public virtual ICollection<StudyMember> StudyMember { get { return _studymember; } }

        public virtual ICollection<TrialRegional> TrialRegional { get { return _trialRegional; } }
       
    }
}
