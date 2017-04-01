using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.View
{
    [Table("StudyListView")]
    public partial class StudyListView
    {
        [Key]
        public Guid Id { get; set; }
        public int? SiteId { get; set; }
        public int? StudyId { get; set; }
        public string Status { get; set; }
        public string StudyNum { get; set; }
        public string ShortTitle { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryStatus { get; set; }
        public string SiteNum { get; set; }
        public string SiteStatus { get; set; }
        public string SiteName { get; set; }
        public string CountryCode { get; set; }
        
    }
}
