using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Settings;
using Com.ETMFS.DataFramework.Interfaces.Settings;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Settings
{
  public partial  class SystemSettingRepository : Repository<SystemConfig, ETMFContext>, ISystemSettingRepository
    {
        public SystemSettingRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }
       public   SystemConfig GetConfigByKey(string key)
        {
           return Dbset.Where(f => f.ConfigKey == key).FirstOrDefault();
        }

       public void AddConfig(SystemConfig config)
       { 
           Dbset.Add(config);
       }
    }
}
