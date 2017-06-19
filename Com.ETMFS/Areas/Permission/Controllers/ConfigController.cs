using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
namespace Com.ETMFS.Areas.Permission.Controllers
{
    public class ConfigController : BaseController
    {
        IStudyService _studyService;
        ISystemSettingService _settingService;
        public ConfigController(IStudyService studyService, ISystemSettingService settingService)
        {
            _studyService = studyService;
            _settingService = settingService;
        }
        // GET: Permission/Config
         [LoginFilter]
        public ActionResult Index()
        {
           return View();
        }



         public JsonResult GetConfigList()
        {
            try
            {
                var xmlentity = _settingService.GetConfig(ConfigList.ConfigXMLPath);
                var config = XMLHelper.ConvertXMLEntity<ConfigSetting>(xmlentity.ConfigXML);
                if (config == null)
                {
                    config = new ConfigSetting();
                }
                return Json(new { Result = true, data = config });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false,Message=ex.Message });
            }
        }


         public JsonResult GetEmailConfig()
         {
             try
             {
                 var xmlentity = _settingService.GetConfig(ConfigList.EmailConfigXMLPath);
                 var config = XMLHelper.ConvertXMLEntity<EmailConfig>(xmlentity.ConfigXML);
                 if (config == null)
                 {
                     config = new EmailConfig();
                 }
                 return Json(new { Result = true, data = config });
             }
             catch (Exception ex)
             {
                 return Json(new { Result = false, Message = ex.Message });
             }
         }

        [LoginFilter]
        [HttpPost]
        public JsonResult MappingFolder()
        {
            try
            {
                var path = this.Server.MapPath(ConfigList.ConfigXMLPath);
                var config = XMLHelper.GetXMLEntity<ConfigSetting>(path );
                if (config != null)
                {
                    _studyService.MappingFolders(config,null);
                }
                return Json(new { Result = true });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false,Message=ex.Message });
            }
           

            
        }
        public JsonResult SaveConfig(ConfigSetting config)
        {
            try{
             var xml= XMLHelper.ConvertEntityXML<ConfigSetting>(config);
             var isdone = _settingService.SaveConfig(ConfigList.ConfigXMLPath, xml);
              if(isdone)
             IdentityScope.Context.ConnectShareFolder(config);
            return Json(new { Result = true });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false,Message=ex.Message });
            }
        }

        public JsonResult SaveEmailConfig(EmailConfig config)
        {
            try
            { 
                 var xml= XMLHelper.ConvertEntityXML<EmailConfig>(config);
                 var isdone = _settingService.SaveConfig(ConfigList.EmailConfigXMLPath, xml);
                return Json(new { Result = true });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }
    }
}