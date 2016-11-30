using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
 public class SiteViewModel
    {
        public int Id { set; get; }
       
        public string SiteNum { set; get; }
        public string SiteName { set; get; }
        public bool? Active { get; set; }
        public string OwnerName { get; set; }
       
        public string CountryName { get; set; }
        
        public int? CountryId { get; set; }
      
        public int? OwnerId { get; set; }
       
        public string Status { get; set; }
        public int StudySiteId { get; set; }

        public int StudyId { get; set; }

        public int MemberId { get; set; }
    }
}
