using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Settings;

namespace Com.ETMFS.DataFramework.Interfaces.Settings
{
  public   interface ISystemSettingRepository
    {
      SystemConfig GetConfigByKey(string key);
      void AddConfig(SystemConfig config); 
    }
}
