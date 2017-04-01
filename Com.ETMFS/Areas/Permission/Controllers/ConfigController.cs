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
        public ConfigController(IStudyService studyService)
        {
            _studyService = studyService;
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
                var path = this.Server.MapPath(ConfigList.ConfigXMLPath);
                var config = XMLHelper.GetXMLEntity<ConfigSetting>(path);
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
                 var path = this.Server.MapPath(ConfigList.EmailConfigXMLPath);
                 var config = XMLHelper.GetXMLEntity<EmailConfig>(path);
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
             var path= this.Server.MapPath(ConfigList.ConfigXMLPath);
             XMLHelper.SaveXMLEntity<ConfigSetting>(path, config);
             IdentityScope.Context.ConnectShareFolder(path);
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
                var path = this.Server.MapPath(ConfigList.EmailConfigXMLPath);
                XMLHelper.SaveXMLEntity<EmailConfig>(path, config);
                return Json(new { Result = true });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
        }
    }
}