using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Areas.MasterData.Controllers
{
    public class StudyController : BaseController
    {
        IStudyService _studyContext;
        IUserService _userService;
        ICountryService _countryService;
        ITMFReferenceService _tmfservice;
        public StudyController(IStudyService studyContext,IUserService userService,ICountryService countryService,ITMFReferenceService tmfservice )
        {  
            _studyContext = studyContext;
            _userService = userService;
            _countryService = countryService;
            _tmfservice = tmfservice;
        }
        // GET: MasterData/Study
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public JsonResult GetStudyList(int page, int rows)
        {
            var users = _studyContext.GetStudyList(page, rows);
            return Json(new { total = users.Total, rows = users.ResultRows });
        }


        [LoginFilter]
        [HttpPost]
        public JsonResult GetUserPermission(TMFFilter filter)
        {
            PermissionViewModel users = null;
            if (CurUser.IsAdministrator)
            {
                users = new PermissionViewModel()
                {
                    IsOwner = true,
                    IsUploader = true,
                    IsReviewer = true
                };
            }
            else
            {
                 users = _studyContext.GetPermission(filter, CurUser.Id);
            }
          
           return Json(users);
        }


        [LoginFilter]
        [HttpPost]
        public JsonResult GetUserStudyList()
        {
            try
            {
                var studylists = _studyContext.GetUserStudyList( CurUser.Id);
                return Json(studylists);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, data = ex.Message });
            }
    
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetUserStudyListView()
        {
            try
            {
                var studylists = _studyContext.GetStudyListView(CurUser.Id);
                return Json(studylists);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, data = ex.Message });
            }

        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetAllTemplates(int id,bool? isCountryLevel,int? countryId, int page, int rows)
        {
            if (id > 0)
            {
                if (!isCountryLevel.Value)
                {
                    var templates = _tmfservice.GetTMFModelList(id, page, rows);
                    return Json(new { total = templates.Total, rows = templates.ResultRows });
                }
                else
                {
                    var templates = _studyContext .GetOutCountryTMFModels(id,countryId.Value, page, rows);
                    return Json(new { total = templates.Total, rows = templates.ResultRows });
                }
                
            }
            else
            {
                return Json(new { total = 0, rows = string.Empty });
            }
            
            
        }
        [LoginFilter]
        [HttpPost]
        public JsonResult SaveStudySite(SiteViewModel site)
        {
            try
            {
                _studyContext.SaveStudySite(site, CurUser.UserName);
                MappingFolder(site.StudyId);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false,message=ex.Message });
            }
            
        }


        [LoginFilter]
        [HttpPost]
        public JsonResult DelStudySites(string sites)
        {
            var vsites = JsonConverter.Deserialize <List<SiteViewModel>>(sites);
            vsites.ForEach(f =>
            {
                _studyContext.SaveStudySite(f, CurUser.UserName);
            });
            return Json(new { result = true });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetTrialSites(int id,int? countryId)
        {
            try
            {
                if (id > 0)
                {
                    var users = _studyContext.GetStudySites(id, countryId,CurUser.Id);
                   
                    return Json(users);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }

        }


        public JsonResult GetAllTrialSites(int id)
        {
            try
            {
                if (id > 0)
                {
                    var users = _studyContext.GetStudySites(id);
                    return Json(users);
                }
                else
                {
                    return Json(string.Empty);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }

        }
         
               [LoginFilter]
        [HttpPost]
        public JsonResult GetAllTrialReginals(int id)
        {
            try
            {
                if (id > 0)
                {
                    var users = _studyContext.GetTrialRegionals(id);
                    return Json(users);
                }
                else
                {
                    return Json(string.Empty);
                }
              
              
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }

        }
        [LoginFilter]
        [HttpPost]
        public JsonResult GetTrialReginals(int id)
        {
            try
            {
                if (id > 0)
                {
                    var users = _studyContext.GetTrialRegionals(id,CurUser.Id);
                    return Json(users);
                }
                else
                {
                    return Json(string.Empty);
                }
              
              
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }

        }
        [LoginFilter]
        [HttpPost]
        public JsonResult GetStudySites(int id, int page, int rows)
        { 

           if (id > 0)
           {
               var templates = _studyContext.GetStudySites(id, page, rows);
               return Json(new { total = templates.Total, rows = templates.ResultRows });
           }
           else
           {
               return Json(new { total = 0, rows = string.Empty });
           }
            
        }


        [LoginFilter]
        [HttpPost]
        public JsonResult SaveStudyTemplates(int id,string studytemps,string countrys,bool isdel)
        {
            try
            {
                var list = this.JsonConverter.Deserialize<List<TMFRefernceOptionViewModel>>(studytemps);
                var temlist = this.JsonConverter.Deserialize<List<int>>(countrys);
                _studyContext.SaveTemplates(id, list, CurUser.UserName, isdel, temlist);
                MappingFolder(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false,message=ex.Message });
            }
         
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult DeleteStudyList(string studyList)
        {
            var list = this.JsonConverter.Deserialize<List<StudyViewModel>>(studyList);
            _studyContext.DeleteStudyList(list, CurUser.UserName);
            return Json(new { result = true });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult SaveStudy(string study)
        {
            try
            {
                var list = this.JsonConverter.Deserialize<StudyViewModel>(study);
                var id = _studyContext.SaveStudyList(list, CurUser.UserName);
                MappingFolder(id);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
           
        }

        void MappingFolder(int? studyId)
        {
            var config = FileHelper.GetConfigSetting(Server, ControllerContext.HttpContext.Application);
            _studyContext.MappingFolders(config, studyId);
        }
        [LoginFilter]
        [HttpPost]
        public JsonResult GetTrialRegionals(int id,int page,int rows)
        {

            var reginals = _studyContext.GetTrialRegionals(id, page, rows);
            return Json(new { total = reginals.Total, rows = reginals.ResultRows });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetTrialRegional(int id, int? countryId)
        {
            try
            {
                if (countryId != null)
                {
                    var reginals = _studyContext.GetTrialRegionals(id);
                    var regional = reginals.FirstOrDefault(f => f.CountryId == countryId);
                    return Json(new { result = true, reginal = regional });
                }
                else
                {
                    return Json(new { result = true });
                }
           
               
            }
            catch (Exception ex)
            {
                return Json(new { result = false,message= ex.Message });
            }
            
        }


        [LoginFilter]
        [HttpPost]
        public JsonResult GetTrialSite(int id, int countryId,int? siteId)
        {
            try
            {
                if (siteId != null)
                {
                    var sites = _studyContext.GetStudySites(id, countryId,p:null);
                    var site = sites.FirstOrDefault(f => f.Id == siteId);
                    return Json(new { result = true, site = site });
                }
                else
                {
                    return Json(new { result = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }

        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetTrialTempaltes(int id, int page, int rows)
        {

            if (id > 0)
            {
                var reginals = _studyContext.GetTrialTempaltes(id, page, rows, CurUser.Id);
                return Json(new { total = reginals.Total, rows = reginals.ResultRows });
            }
            else
            {
                return Json(new { total = 0, rows = string.Empty });
            }

           
        }


        [LoginFilter]
        [HttpPost]

        public JsonResult GetAllTrialTempaltes(TMFFilter codition)
        {
            try
            {
                var reginals = _studyContext.GetTrialTempaltes(codition);
                return Json(reginals);
            }
            catch (Exception ex)
            {
                return Json(string.Empty);
            }
        }
        [LoginFilter]
        [HttpPost]
        public JsonResult SaveTrialRegionals(TrialReginalViewModel trialReg)
        {
            try
            {
                _studyContext.SaveTrialRegional(trialReg, CurUser.UserName);
                MappingFolder(trialReg.StudyId);
                return Json(new { result = true });
            }catch(Exception ex){
               return Json(new { result = false,Messsage=ex.Message });   
            }
         
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetUserList( )
        {
            var users = _userService.GetUserList();

            return Json(users);
        }
        [LoginFilter]
        [HttpPost]
       public JsonResult GetMembers(int id,int page,int rows,int? countryId,int? siteId){
           try
           {
               var users = _studyContext.GetStudyMembers(id, page, rows, countryId, siteId);
               return Json(new { total = users.Total, rows = users.ResultRows });
           }
           catch (Exception ex)
           {
               return Json(new { result = false, Message = ex.Message });
           }

        }

        [LoginFilter]
        [HttpPost]
        public JsonResult SaveMember(MemberViewModel mem)
        {
            try
            {
                _studyContext.SaveStudyMembers(mem,CurUser.UserName);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }

        }

        [LoginFilter]
        [HttpPost]
        public JsonResult DeleteMembers(string  mems)
        {
            try
            {
                var memList = JsonConverter.Deserialize<List<MemberViewModel>>(mems);
                if(memList.Count>0)
                _studyContext.RemoveStudyMembers(memList, CurUser.UserName);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }

        }
        

        [LoginFilter]
        [HttpPost]
        public JsonResult GetCountryList()
        {
            try
            {
                var users = _countryService.GetCountries();
                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(new { result = false,Message=ex.Message });
            }
        }
         [LoginFilter]
        public JsonResult GetOptionListByParentId(int id)
        {
            try
            {
                var users = _studyContext.GetOptionListByParentId(id);
                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = ex.Message });
            }
        }
          
        [LoginFilter]
        [HttpPost]
        public JsonResult DeleteTrialRegionals(string trialRegs)
        {
            var trialReg = JsonConverter.Deserialize<List<TrialReginalViewModel>>(trialRegs);
            _studyContext.RemoveRegionals(trialReg, CurUser.UserName);
            return Json(new { result = true });
        }
    }
}