using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Com.ETMFS.BusinesService.ViewModel.Permission;

namespace Com.ETMFS.Areas
{
    public class BaseController : Controller
    {
        // GET: Base
        UserViewModel _curuser = null;
        JavaScriptSerializer _scriptconventor = new JavaScriptSerializer();
         public  UserViewModel CurUser
        {
            get { return _curuser = Session["User"] as UserViewModel; }
        }

         public JavaScriptSerializer JsonConverter
         {
             get{return _scriptconventor;}
         }
      
    }
}