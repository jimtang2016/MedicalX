using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.BusinesService.Interfaces;
using Com.ETMFS.BusinesService.ViewModel.Permission;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Areas.MasterData.Controllers
{
    class DocumentType
    {
    public static string   PPT="PPTX";
    public static string Excel = "XLSX";
    public static string Word = "DOCX";
    public static string Word2003 = "DOC";
    public static string Excel2003 = "XLS";
    public static string PPT2003 = "PPT";
    public static string PDF = "PDF";
    }
    public class StudyDocumentController : BaseController
    {
        IDocumentService _documentService;
        IUserService _userservice;
        IStudyService _studyservice;
        public StudyDocumentController(IDocumentService documentService, IUserService userservice, IStudyService studyservice)
        {
            _documentService = documentService;
            _userservice = userservice;
            _studyservice = studyservice;
        }
        // GET: MasterData/StudyDocument
        [LoginFilter]
        public ActionResult Index()
        {
            ViewBag.CurrentUser = CurUser.UserName;
            ViewBag.CurrentUserId = CurUser.Id;
            return View();
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetSumView(TMFFilter condition)
        {
         DocumentSumView   uploadSum=_documentService.GetUploadConculation( CurUser.Id, condition);
         DocumentSumView   reviewSum=_documentService.GetReviewConculation( CurUser.Id, condition);
            return Json(new{UploadSum=uploadSum,ReviewSum=reviewSum});
        }

     

        [LoginFilter]
        [HttpPost]
        public JsonResult GetDocumentList(int page,int rows,TMFFilter condition)
        {
            try
            {
                if (condition == null)
                    condition = new TMFFilter();
                var list = _documentService.GetDocumentList(page, rows, CurUser.Id, condition);
                return Json(new { total = list.Total, rows = list.ResultRows });
            }
            catch (Exception ex)
            {
                return Json(new { ex=ex.ToString()});
            }
           
        }
        string GetFilePath(ConfigSetting config, TMFFilter tmf,string filename)
        {
          
            var splitflag = Constant.Document_FileSplitFlag;
            if (config.HostType == (int)HostType.SharePoint)
            {
                splitflag = Constant.Document_WebSplitFlag;
            }

            var path = config.PathURI  + config.RootFolder + splitflag + tmf.StudyNum;

            if (tmf.Country.HasValue)
            {
                path = path + splitflag + tmf.CountryName;
            }
            if (tmf.Site.HasValue)
            {
                path = path + splitflag + tmf.SiteName;
            }

            path = path + splitflag + tmf.ZoneNo + splitflag + tmf.SectionNo + splitflag + tmf.ArticleNo + splitflag + filename;

            return path;
        }
         void SaveDocument(TMFFilter tmf,string filename, HttpPostedFileBase file )
        {
              
            var config = FileHelper.GetConfigSetting(Server,ControllerContext.HttpContext.Application);
            string path = GetFilePath(config,tmf, filename);
            var helpers = new FileHelper();
            if (config.HostType == (int)HostType.SharePoint)
            {
                helpers.UploadtoSharePoint(path, config, file);
            }
            else
            {
                helpers.UploadtoFileSystem(path, file);
            
            }
      
           
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult UpateLoadDocument(TMFFilter tmf, HttpPostedFileBase file)
        {
            try
            {
                if (tmf != null)
                {
                    if (file != null)
                    {
                        var splitflag = Constant.Document_TypeSplitFlag;
                        var lastindex = file.FileName.LastIndexOf(splitflag);
                        tmf.DocumentName = file.FileName.Substring(0, lastindex);
                        tmf.DocumentType = file.FileName.Substring(lastindex + 1, file.FileName.Length - lastindex - 1);
                        var filename = tmf.DocumentName + tmf.VersionId
                            + splitflag + tmf.DocumentType;
                        SaveDocument(tmf, filename, file);
                    }

                    _documentService.SaveDocument(tmf, CurUser.Id, CurUser.UserName);
                }
                return Json(new { result = true  });

            }
            catch (Exception ex)
            {
                return Json(new {result = false, ex = ex.Message });
            }

        }


        [LoginFilter]
        [HttpPost]
        public JsonResult GetStudyListView(int page, int rows)
        {
            try
            {
                var users = _studyservice.GetStudyListView(CurUser.Id);
                return Json(users);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
      
        }
        [LoginFilter]
        public FileResult DownloadFile(string tmfilter)
        {
            var config = FileHelper.GetConfigSetting(Server, HttpContext.Application);
            try
            {
                FileHelper helpers = new FileHelper();
                byte[] temp;
                var tmf = JsonConverter.Deserialize<TMFFilter>(tmfilter);
                var filename = tmf.DocumentName + tmf.VersionId
                    + Constant.Document_TypeSplitFlag + tmf.DocumentType;
                string path = GetFilePath(config, tmf, filename);
                if (config.HostType == (int)HostType.SharePoint)
                {
                    temp = helpers.DownloadWebServerFile(path, config);
                }
                else
                {
                    temp = helpers.DownloadfromFileSystem(path);
                }
                var contenttype = Constant.ContentTypeLib[Constant.Document_ContentType_Other];
                if (Constant.ContentTypeLib.Keys.Contains(tmf.DocumentType))
                {
                    contenttype = Constant.ContentTypeLib[tmf.DocumentType];
                }
                return File(temp, contenttype, filename);
            }
            catch (Exception ex)
            {
                return null;
            }
           
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetDocumentHistory(int? documentId,int page, int rows )
        {
           
            if (documentId.HasValue)
            {
                var list = _documentService.GetDocumentHistory(page, rows, documentId.Value);
                return Json(new { total = list.Total, rows = list.ResultRows });
            }
            else
            {
                var list = new List<DocumentViewModel>();
                return Json(new { total = 0, rows = list });
            }
           
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetIssuelogs(int? documentId, int page, int rows, bool?isAllIssues,string status)
        {
            if (documentId.HasValue)
            {
                var list = _documentService.GetIssueLogs(page, rows, documentId.Value,isAllIssues.Value, status);
                return Json(new { total = list.Total, rows = list.ResultRows });
            }
            else
            {
                var list = new List<DocumentViewModel>();
                return Json(new { total = 0, rows = list });
            }

        }
        [LoginFilter]
        public JsonResult GetDocumentUser(string tmfilter)
        {
            var tmf = JsonConverter.Deserialize<TMFFilter>(tmfilter);
            List<UserViewModel> users = new List<UserViewModel>();
            if (tmf != null)
            {
                users = _userservice.GetDocumentUserList(tmf);
            }
            return Json(users);
        }
        public JsonResult GetPDFFile(string tmfilter)
        {
            var mapfolder="/TempFiles/";
            var config = FileHelper.GetConfigSetting(Server, HttpContext.Application);
            try
            {
                FileHelper helpers = new FileHelper();
                byte[] temp;
                var tmf = JsonConverter.Deserialize<TMFFilter>(tmfilter);
                var filename = tmf.DocumentName + tmf.VersionId
                    + Constant.Document_TypeSplitFlag + tmf.DocumentType;
                string path = GetFilePath(config, tmf, filename);
                if (config.HostType == (int)HostType.SharePoint)
                {
                    temp = helpers.DownloadWebServerFile(path, config);
                }
                else
                {
                    temp = helpers.DownloadfromFileSystem(path);
                }
                var temppath = Server.MapPath(mapfolder);
                temppath = temppath + filename;
                using (System.IO.FileStream file = new System.IO.FileStream(temppath, FileMode.OpenOrCreate))
                {
                    file.Write(temp, 0, temp.Length);
                }

                if (tmf.DocumentType.ToUpper() == DocumentType.Word || tmf.DocumentType.ToUpper() == DocumentType.Word2003)
                {
                    helpers.ConvertFromWord(temppath, temppath  + ".pdf");
                }else
                    if (tmf.DocumentType.ToUpper() == DocumentType.Excel || tmf.DocumentType.ToUpper() == DocumentType.Excel2003)
                {
                    helpers.ConvertFromExcel(temppath, temppath+ ".pdf");
                } if (tmf.DocumentType.ToUpper() == DocumentType.PPT || tmf.DocumentType.ToUpper() == DocumentType.PPT2003)
                {
                    helpers.ConvertFromPPT(temppath, temppath + ".pdf");
                }

                if (tmf.DocumentType.ToUpper() != DocumentType.PDF)
                {
                    filename = filename + ".pdf";
                }

                return Json(new { result = true, url = "http://" + Request.Url.Host + ":" + Request.Url.Port + mapfolder + filename });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }

        }

        [LoginFilter]
        [HttpPost]
        public JsonResult SaveDocument(string tmf)
        {
         
            try 
            {
                if (tmf != null)
                {
                    EmailHelper.Current.LoadConfig(Server);
                    List<TMFFilter> tmfs = JsonConverter.Deserialize<List<TMFFilter>>(tmf);
                    tmfs.ForEach(tft =>
                    {
                        _documentService.SaveDocument(tft, CurUser.Id, CurUser.UserName);
                    });
                    
                }
                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                return Json(new {result = false,  ex = ex.Message });
            }

        }
        
    }
}