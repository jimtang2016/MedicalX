using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.BusinesService.ViewModel.Permission;
using Com.ETMFS.Service.Common;

namespace Com.ETMFS.Areas.Permission.Controllers
{
    public class GroupController : BaseController
    {
        IGroupService _groupservice;
        JavaScriptSerializer _scriptconventor = new JavaScriptSerializer();
       
        public GroupController(IGroupService groupservice):base()
        {
            _groupservice = groupservice;
            
        }
        // GET: Permission/Group
          [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GroupList(int page, int rows, string searchConditions)
        {
            var grouplist=_groupservice.GetGroupList(page,rows,searchConditions);
            return Json(new { total = grouplist.Total, rows = grouplist.ResultRows });
        }


         [HttpPost]
        public JsonResult GroupUserList(int page, int rows, int id)
        {
            var grouplist = _groupservice.GetUserList(page, rows, id);
            return Json(new { total = grouplist.Total, rows = grouplist.ResultRows });
        }
        

        [HttpPost]
        public JsonResult SaveGroup(  string group)
        {
            try
            {
                if ( CurUser != null)
                {
                    var vgroups = _scriptconventor.Deserialize<UserGroupViewModel>(group);
                    _groupservice.SaveGroup(vgroups, this.CurUser.UserName);
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false, message = MessageContant.UserInvaid });
                }
            }
            catch (Exception ex)
            {
                return Json(new   { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult RemoveGroups(string group)
        {
            try
            {

                if (CurUser != null)
                {
                    var vgroups = _scriptconventor.Deserialize<List<UserGroupViewModel>>(group);
                    _groupservice.RemoveGroups(vgroups, CurUser.UserName);
                    return Json(new { result = true });
                }
                else
                {
                    return Json(new { result = false, message = MessageContant.UserInvaid });
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
    }
}