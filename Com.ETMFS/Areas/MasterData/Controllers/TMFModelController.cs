using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Areas.MasterData.Controllers
{
    public class TMFModelController : BaseController
    {
        ITMFReferenceService _tmfservice;
        // GET: MasterData/TMFModel
        public TMFModelController(ITMFReferenceService tmfservice)
        {
            _tmfservice = tmfservice;
        }
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetTMFList(int page, int rows)
        {
            var users = _tmfservice.GetTMFModelList(page, rows);
            return Json(new { total = users.Total, rows = users.ResultRows });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult SaveTMF (TMFRefernceViewModel template)
        {
            _tmfservice.SaveTMFReference(template,CurUser.UserName);
            return Json(new { result=true });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult DeleteTMFs(string templates)
        {
            var dellist=JsonConverter.Deserialize<List<TMFRefernceViewModel>>(templates);
            _tmfservice.DeleteTMFReference(dellist, CurUser.UserName);
            return Json(new { result = true });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult Import(HttpPostedFileBase file)
        {
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(file.FileName));
            file.SaveAs(fileName);
            var datatable = ExcelHelper.ConvertToDataTable(fileName, 30);
            _tmfservice.SaveTMFReferences(datatable,CurUser.UserName);
            return Json(new { result = true });
        }

        [LoginFilter]
        public FileResult ExportExcel()
        {
              var sbHtml = new StringBuilder();
              var fileContents = _tmfservice.GetAllTemplateStream();
             
              return File(fileContents, "application/ms-excel", "fileContents.csv");
        }
    }
}