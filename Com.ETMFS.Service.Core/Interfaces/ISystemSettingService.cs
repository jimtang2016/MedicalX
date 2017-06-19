using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Settings;

namespace Com.ETMFS.Service.Core.Interfaces
{
    public interface ISystemSettingService
    {
        SystemConfig GetConfig(string key);
        bool SaveConfig(string key,string xmlContent);
    }
}
