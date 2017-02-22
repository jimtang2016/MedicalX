using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.Service.Common;

namespace Com.ETMFS.BusinesService.ViewModel.Permission
{
    [Serializable]
  public  class UserViewModel
    {
      List<UserGroupViewModel> _groups = null;
      public UserViewModel()
      {
          _groups = new List<UserGroupViewModel>();
      }
      public int Id { set; get; }
      public string UserName { set; get; }
      public string Password { set; get; }
      public string Country { set; get; }
      public string IpAddress { set; get; }
      public string ModifiBy { set; get; }
      public string CreateBy { set; get; }
      public List<UserGroupViewModel> Groups { get { return _groups; } }
      public int OPStatus { set; get; }
      public bool IsReviewer{ get { return _groups.Any(f => f.Id == Constant.Group_Reviewers); } }
      public bool IsUploader { get { return _groups.Any(f => f.Id == Constant.Group_Uploaders); } }
      public bool IsAdministrator { get { return _groups.Any(f => f.Id == Constant.Group_Administrators); } }
    }
}
