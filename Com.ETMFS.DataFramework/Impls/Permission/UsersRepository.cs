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
    public class UsersRepository : Repository<Users, ETMFContext>,IUsersRepository
    {
        public UsersRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }

        #region IUsersRepository Members

        public Users GetUserByName(string userName)
        {
            var user = Dbset.Where(f => f.UserName == userName&&f.Active.Value).FirstOrDefault();
          return user;
        }

        public bool SaveLoginHistory(int userid,string ipAddress)
        {
            try { 
            var usere = GetById(userid);
            var hos=usere.LoginHistory.FirstOrDefault(f=>!f.LogOffTime.HasValue);

            if(hos==null){
                  hos=new LoginHistory(){
                  LoginIpAddress=ipAddress,
                  LoginTime=DateTime.Now,
                   Users=usere 
            };

                  usere.LoginHistory.Add(hos);
            }
            else
            {
                hos.LogOffTime = DateTime.Now;
                var dur = (hos.LogOffTime - hos.LoginTime);
                if(dur!=null)
                hos.Duration = dur.Value.Minutes ;
            }
            
            return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        #endregion

        #region IUsersRepository Members

     public   List<Users> GetActiveUserList()
        {
          return  Dbset.Where(f => f.Active.HasValue && f.Active.Value).ToList();
        }
        public PageResult<Users> GetUserList(PageResult<Users> page)
        {
            var pagel = page;
            pagel.ResultRows.AddRange(Dbset.Where(f => f.Active.Value).OrderBy(f=>f.Id).Skip(pagel.SkipCount).Take(pagel.PageSize).ToList());
            pagel.Total = Dbset.Count(f => f.Active.Value);
            return pagel;
        }

        #endregion
    }
}
