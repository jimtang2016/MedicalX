using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
   public class PermissionViewModel
    {
       public bool IsOwner { get; set; }
       public bool IsUploader { get; set; }
       public bool IsReviewer { get; set; }
       public bool IsAdministrator { get; set; }
       public bool IsSiteOwner { get; set; }
       public bool IsCountryOwner { get; set; }
       public bool IsStudyOwner { get; set; }

       public bool IsStudyReviewer { get; set; }

       public bool IsCountryReviewer { get; set; }

       public bool IsSiteReviewer { get; set; }

       public bool IsSiteUploader { get; set; }

       public bool IsCountryUploader { get; set; }

       public bool IsStudyUploader { get; set; }
    }
}
