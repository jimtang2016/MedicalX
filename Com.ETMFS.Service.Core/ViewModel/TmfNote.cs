using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;

namespace Com.ETMFS.Service.Core.ViewModel
{
  public  class TmfNote
    {
      List<TmfNote> _children = new List<TmfNote>();
      public string id { set; get; }
      public string text { set; get; }
      public string iconCls { set; get; }
      public List<TmfNote> children { get { return _children; } }
      public TMFFilter category { set; get; }
      public string RoleLevel { set; get; }
    }
}
