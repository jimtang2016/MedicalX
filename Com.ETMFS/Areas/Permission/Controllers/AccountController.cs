using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.BusinesService.ViewModel.Permission;

namespace Com.ETMFS.Areas.Permission.Controllers
{
    public class AccountController : BaseController
    {
        IUserService _userService = null;
       
        
        public AccountController(IUserService userService):base()
        {
            _userService = userService;
        }
         
        // GET: Permission/Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [LoginFilter]
        public ActionResult UserList()
        {
            return View();
        }
         [HttpPost]
         
        public JsonResult UserList(int page, int rows, string searchConditions)
        {
            var users = _userService.GetUserList(page, rows, searchConditions);
            return Json(new { total=users.Total,rows=users.ResultRows });
                
        }


         [HttpPost]

         public JsonResult GetGroupsByUserId(int page, int rows, int id)
         {
             var users = _userService.GetGroupsByUserId(page, rows, id);
             return Json(new { total = users.Total, rows = users.ResultRows });

         }

            [HttpPost]
            public JsonResult SaveUser(UserViewModel user)
            {
                try
                {

                    if (CurUser != null)
                    {
                        user.CreateBy = CurUser.UserName;
                        user.ModifiBy = CurUser.UserName;
                        _userService.SaveUser(user);
                        return Json(new { result = true });
                    }
                    else
                    {
                        return Json(new { result = false });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { result = false,message=ex.Message });
                }
                
               
            }

            [HttpPost]
            public JsonResult RemoveUsers(string  users)
            {
                try
                {
                    if (CurUser != null)
                    {
                        var useres = JsonConverter.Deserialize<List<UserViewModel>>(users);
                        _userService.RemoveUsers(useres, CurUser.UserName);
                        return Json(new { result = true });
                    }
                    else
                    {
                        return Json(new { result = false });
                    }
                 
                }
                catch (Exception ex)
                {
                    return Json(new { result = false, message = ex.Message });
                }


            }

        [HttpPost]
        public JsonResult Login(string logindata)
        {
            try
            {
                var userv = JsonConverter.Deserialize<UserViewModel>(logindata);
                var user = _userService.LogIn(userv.UserName, userv.Password, Request.UserHostAddress);
                if (user.Id>0)
                {
                    Session.Add("User", user);
                    return Json(new { Result = true, Message = "Wellcome to onboard" });
                }
                else
                {
                    return Json(new { Result = false, Message = "User Name or Password error" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }
         
        }


        [HttpPost]
        public JsonResult LogOff( )
        {
            try
            {
                
                if (CurUser!=null)
                {
                    _userService.LogOff(CurUser);
                    Session.Remove("User");
                    return Json(new { Result = true, Message = "Logoff successfully" });
                }
                else
                {
                    return Json(new { Result = false, Message = "User did not login" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.Message });
            }

        }
    }
}