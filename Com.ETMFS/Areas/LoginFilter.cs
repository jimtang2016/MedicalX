using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.BusinesService.ViewModel.Permission;

namespace Com.ETMFS.Areas
{
    public class LoginFilter : FilterAttribute, IActionFilter
    {

        #region IActionFilter Members

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user =filterContext.HttpContext.Session["User"] as UserViewModel;
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/Permission/Account/Index");  
            }
            else
            {
                filterContext.Controller.ViewBag.CurrentUser=user.UserName;
            }
        }

        #endregion
    }
}