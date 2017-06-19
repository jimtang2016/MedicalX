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
    [Table("T_TMFTemplate")]
    public partial class TMFTemplate
    {
         ICollection<StudyTemplate> _studytemplate = null;
         public TMFTemplate()
         {

             _studytemplate = new HashSet<StudyTemplate>();
         }
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [StringLength(500)]
        public string ZoneNo { get; set; }
        [StringLength(500)]
        public string ZoneName { get; set; }
        [StringLength(500)]
        public string SectionNo { get; set; }
        [StringLength(500)]
        public string SectionName { get; set; }
        [StringLength(500)]
        public string ArtifactNo { get; set; }
        [StringLength(500)]
        public string ArtifactName { get; set; }
        [StringLength(500)]
        public string AlternateNames { get; set; }
        public string Purpose { get; set; }
        [StringLength(500)]
        public string Inclusion { get; set; }
        [StringLength(500)]
        public string ICHCode { get; set; }
        [StringLength(500)]
        public string EDMName { get; set; }
          [StringLength(500)]
        public string SubArtifacts { get; set; }
        [StringLength(100)]
        public string UniqueID { get; set; }

       [StringLength(500)]
        public string DeviceInvestRequired { get; set; }
        [StringLength(500)]
        public string DeviceSponsorRequired { get; set; }

        [StringLength(500)]
        public string NonDeviceSponsorRequired { get; set; }
        [StringLength(500)]
        public string NonDeviceInvestRequired { get; set; }
        [StringLength(500)]
        public string InvestigatorInitated { get; set; }
        [StringLength(500)]
        public string IsBaseMetaData { get; set; }
        [StringLength(500)]
        public string ProcessNumber { get; set; }
        [StringLength(500)]
        public string ProcessName { get; set; }
        [StringLength(500)]
        public string IsTrialLevel { get; set; }
        [StringLength(500)]
        public string IsCountryLevel { get; set; }
        [StringLength(500)]
        public string IsSiteLevel { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Created { get; set; }
        [StringLength(100)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? Modified { get; set; }
        [StringLength(100)]
        public string ModifiBy { get; set; }
       

        public virtual ICollection<StudyTemplate> StudyTemplate { get { return _studytemplate; } }
    }
}
