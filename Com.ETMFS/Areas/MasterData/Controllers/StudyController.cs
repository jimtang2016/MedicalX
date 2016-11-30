using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.BusinesService.Interfaces;
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
        public JsonResult GetAllTemplates(int id, int page, int rows)
        {
            if (id > 0)
            {
                var templates = _tmfservice.GetTMFModelList(id, page, rows);
                return Json(new { total = templates.Total, rows = templates.ResultRows });
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
            _studyContext.SaveStudySite(site,CurUser.UserName);

            return Json(new { result = true });
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
        public JsonResult GetTrialSites(int id)
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
        public JsonResult GetTrialReginals(int id)
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
            var list = this.JsonConverter.Deserialize<List<TMFRefernceViewModel>>(studytemps);
            var temlist = this.JsonConverter.Deserialize<List<int>>(countrys);
            _studyContext.SaveTemplates(id, list, CurUser.UserName, isdel, temlist);
            return Json(new { result = true });
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
            var list = this.JsonConverter.Deserialize<StudyViewModel>(study);
            _studyContext.SaveStudyList(list, CurUser.UserName);
            return Json(new { result = true });
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
        public JsonResult SaveTrialRegionals(TrialReginalViewModel trialReg)
        {
            try
            {
                _studyContext.SaveTrialRegional(trialReg, CurUser.UserName);
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
       public JsonResult GetMembers(int id,int page,int rows){
           try
           {
               var users = _studyContext.GetStudyMembers(id, page, rows);
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