using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Permission;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Permission
{
    public class UserGroupsRepository : Repository<UserGroups, ETMFContext>, IUserGroupsRepository
    {
        public UserGroupsRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }

        #region IUserGroupsRepository Members

        public PageResult<UserGroups> GetGroupList(PageResult<UserGroups> page)
        {
            var pagel = page;
            pagel.ResultRows.AddRange(Dbset.Where(f => f.Active.Value).OrderBy(f => f.Id).Skip(pagel.SkipCount).Take(pagel.PageSize).ToList());
            pagel.Total = Dbset.Count(f => f.Active.Value);
            return pagel;
        }

        #endregion
    }
}
