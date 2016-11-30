using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.BusinesService.ViewModel.Permission;
using Com.ETMFS.DataFramework;

namespace Com.ETMFS.BusinesService.Interfaces
{
   public interface IGroupService 
    {
       PageResult<UserGroupViewModel> GetGroupList(int currentPage, int pageSize, string searchConditions);

       void SaveGroup(UserGroupViewModel vgroups,string username);

       void RemoveGroups(List<UserGroupViewModel> vgroups, string username);

       PageResult<UserViewModel> GetUserList(int page, int rows, int id);
    }
}
