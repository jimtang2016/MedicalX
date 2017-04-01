using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
   public class TMFRefernceViewModel
    {
        public int Id { get; set; }
         
        public string ZoneNo { get; set; }
         
        public string ZoneName { get; set; }
       
        public string SectionNo { get; set; }
        
        public string SectionName { get; set; }
        
        public string ArtifactNo { get; set; }
         
        public string ArtifactName { get; set; }

        public string AlternateNames { get; set; }
        public string Purpose { get; set; }
        
        public string Inclusion { get; set; }
        public string SubArtifacts { get; set; }
        public string ICHCode { get; set; }
       
        public string EDMName { get; set; }
      
        public string UniqueID { get; set; }

        public string DeviceSponsorRequired { get; set; }
        public string DeviceInvestRequired { get; set; }
        public string NonDeviceSponsorRequired { get; set; }
        public string NonDeviceInvestRequired { get; set; }
      
        public string InvestigatorInitated { get; set; }
        
        public string IsBaseMetaData { get; set; }
         
        public string ProcessNumber { get; set; }
         
        public string ProcessName { get; set; }
        
        public string IsTrialLevel { get; set; }
         
        public string IsCountryLevel { get; set; }
        
        public string IsSiteLevel { get; set; }

        public bool? Active { get; set; }
        
        public DateTime? Created { get; set; }
        
        public string CreateBy { get; set; }
 
        public DateTime? Modified { get; set; }
       
        public string ModifiBy { get; set; }
    }

   public class TMFRefernceOptionViewModel
   {
       public int Id { get; set; }

       public string ZoneNo { get; set; }

       public string ZoneName { get; set; }

       public string SectionNo { get; set; }

       public string SectionName { get; set; }

       public string ArtifactNo { get; set; }

       public string ArtifactName { get; set; }

       public string AlternateNames { get; set; }
       public string Purpose { get; set; }
       public string IsTrialLevel { get; set; }

       public string IsCountryLevel { get; set; } 
       public string IsSiteLevel { get; set; }
       public int StudyTemplateId { get; set; }
   }
}
