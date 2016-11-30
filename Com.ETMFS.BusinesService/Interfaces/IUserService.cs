using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.BusinesService.ViewModel.Permission;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Permission;

namespace Com.ETMFS.BusinesService.Interfaces
{
  public  interface IUserService
    {
      UserViewModel LogIn(string userId, string password,string ipAddress);

      bool LogOff(UserViewModel user);

      PageResult<UserViewModel> GetUserList(int currentPage, int pageSize, string searchConditions);

      void SaveUser(UserViewModel user);
      List<UserViewModel> GetUserList();
      void RemoveUsers(List<UserViewModel> useres,string curuser);

      PageResult<UserGroupViewModel> GetGroupsByUserId(int page, int rows, int id);
    }
}
