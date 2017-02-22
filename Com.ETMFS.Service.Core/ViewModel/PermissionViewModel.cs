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
       
    }
}
