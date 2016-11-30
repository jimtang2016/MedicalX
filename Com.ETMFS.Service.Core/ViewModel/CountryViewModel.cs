using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
 public   class CountryViewModel
    {
     public int Id { get; set; }
     public string CountryCode { get; set; }
     public string CountryName { get; set; }
     public bool? Active { get; set; }
    }
}
