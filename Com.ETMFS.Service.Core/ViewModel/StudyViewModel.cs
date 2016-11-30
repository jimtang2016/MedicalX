using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.ETMFS.Service.Core.ViewModel
{
  public  class StudyViewModel
    {
      public int Id { set; get; }
      public string StudyNum { set; get; }
      public string ShortTitle { set; get; }
      public string Status { set; get; }
      public DateTime? Modified { set; get; }
      public DateTime? Created { set; get; }
      public string CreateBy { set; get; }
      public string ModifiBy { set; get; }
      public bool? Active { set; get; }
    }
}
