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
    [Table("T_Site")]
    public partial class Site
    {
        ICollection<StudySite> _studysite = null;
        public Site()
        {
            _studysite = new HashSet<StudySite>();
        }
        public int Id { set; get; }
        [StringLength(100)]
        public string SiteNum { set; get; }
        [StringLength(100)]
        public string SiteName { set; get; }
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
 

        [NotMapped]
        public string OwnerName { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public int? CountryId { get; set; }
        [NotMapped]
        public int? OwnerId { get; set; }
        [NotMapped]
        public string Status  { get; set; }
         [NotMapped]
        public int StudySiteId { get; set; }
         [NotMapped]
         public int MemberId { get; set; }
    }
}
