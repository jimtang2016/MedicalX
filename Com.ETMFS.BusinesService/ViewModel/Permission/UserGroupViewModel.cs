using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.BusinesService.ViewModel.Permission
{
   public  class UserGroupViewModel
    {

       public int Id { get; set; }
       public string  GroupName { get; set; }
       public string Description { get; set; }
       public List<UserViewModel> GUsers { get; set; }
     
    }
}
