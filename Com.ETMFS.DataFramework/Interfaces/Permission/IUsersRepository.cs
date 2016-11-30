using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Permission;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Permission
{
   public interface IUsersRepository:  IRepository<Users>
    {
       Users GetUserByName(string userName);

       bool SaveLoginHistory(int userid, string ipAddress);

       PageResult<Users> GetUserList(PageResult<Users> page );

       List<Users> GetActiveUserList();
    }
}
