using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Com.ETMFS.DataFramework.Entities.Settings;
using Com.ETMFS.DataFramework.Interfaces.Settings;
using Com.ETMFS.Service.Core.Interfaces;
using RepositoryT.Infrastructure;
 
namespace Com.ETMFS.Service.Core.Impls
{
    public partial class SystemSettingService : ISystemSettingService
    {
        #region ISystemSettingService Members
        ISystemSettingRepository _configrepos;
        IUnitOfWork _unitwork;
        public SystemSettingService(ISystemSettingRepository configrepos,IUnitOfWork unitwork)
        {
            _configrepos = configrepos;
            _unitwork = unitwork;
        }
        public SystemConfig GetConfig(string key) 
        {
            var config =  HttpRuntime.Cache[key] as SystemConfig ;
            if( config==null){
                config = _configrepos.GetConfigByKey(key);
                if(config!=null)
                HttpRuntime.Cache.Insert(key, config);
                else
                {
                    config = new SystemConfig();
                }
            }
            return config;
        }

        public bool SaveConfig(string key, string xmlContent)
        {
            try
            {
                var config = GetConfig(key);
                if (config.Id<=0)
                {
                    config = new SystemConfig()
                    {
                        ConfigKey = key,
                        ConfigXML = xmlContent
                    };

                }
                else
                {
                    config.ConfigXML = xmlContent;
                }
                if (config.Id > 0)
                {
                    HttpRuntime.Cache.Remove(key);
                }
                else
                {
                    _configrepos.AddConfig(config);
                }
                _unitwork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}
