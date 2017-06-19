using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.DataFramework.View
{
    [Table("NotifyReminderView")]
    public partial class NotifyReminderView
    {

        public int Id { get; set; }
        public string StudyNum { get; set; }
        public string ShortTitle { get; set; }
        public string CountryName { get; set; }
        public string SiteName { get; set; }
        public string SiteNum { get; set; }
        public string ArtifactName { get; set; }
        public DateTime? RunDate { get; set; }
    }
}
