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
    [Table("T_StudySite")]
    public partial class StudySite
    {

        public int Id { get; set; }
        public int? SiteId { get; set; }
        public int? StudyId { get; set; }
        public int? OwnerId { get; set; }
        public int? CountryId { get; set; }
        public virtual Site Site { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
        public virtual Study Study { get; set; }
    }
}
