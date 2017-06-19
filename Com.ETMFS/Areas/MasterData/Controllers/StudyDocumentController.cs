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
using Com.ETMFS.DataFramework.Interfaces.Settings;
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
        ISystemSettingRepository _settingService;
        public StudyDocumentController(IDocumentService documentService, IUserService userservice, IStudyService studyservice,ISystemSettingRepository settingService)
        {
            _documentService = documentService;
            _userservice = userservice;
            _studyservice = studyservice;
            _settingService = settingService;
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
         DocumentSumView   issueSum = _documentService.GetIssuedConculation(CurUser.Id, condition);
         return Json(new { UploadSum = uploadSum, ReviewSum = reviewSum, IssueSum = issueSum });
        }

        [LoginFilter]
        [HttpPost]
        public JsonResult GetTopDocumentList( TMFFilter condition,int top,int type)
        {
            TopType toptype=(TopType)type;
            var list = _documentService.GetTopTmfDocuments(CurUser.Id, condition, top, toptype);
            return Json(new {result=true,list=list});
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
        string GetFilePath(ConfigSetting config, DocumentViewModel tmf, string filename)
        {

            var splitflag = Constant.Document_FileSplitFlag;
            if (config.HostType == (int)HostType.SharePoint)
            {
                splitflag = Constant.Document_WebSplitFlag;
            }

            var path = config.PathURI + config.RootFolder + splitflag + tmf.StudyNum;

            if (!string.IsNullOrEmpty(tmf.CountryName))
            {
                path = path + splitflag + tmf.CountryName;
            }
            if (!string.IsNullOrEmpty(tmf.SiteName))
            {
                path = path + splitflag + tmf.SiteName;
            }

            path = path + splitflag + tmf.ZoneNo + splitflag + tmf.SectionNo + splitflag + tmf.ArtifactNo + splitflag + filename;

            return path;
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
            var sysconfig = _settingService.GetConfigByKey(ConfigList.ConfigXMLPath);
            var config = XMLHelper.ConvertXMLEntity<ConfigSetting>(sysconfig.ConfigXML);
            
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

                    _documentService.SaveDocument(tmf, CurUser.Id, CurUser.UserName,true);
                }
                return Json(new { result = true  });

            }
            catch (Exception ex)
            {
                return Json(new {result = false, message = ex.Message });
            }

        }
        [LoginFilter]
        [HttpPost]
        public JsonResult GetNotifyRules(int? studyId,int? countryId,int? siteId,int? tmfId, int page, int rows)
        {
            try
            {
                if(studyId.HasValue){
                    var list = _documentService.GetNotifyRules(studyId, countryId, siteId, tmfId, page, rows);
                    return Json(new { total = list.Total, rows = list.ResultRows });
                }
                
                else
                   return Json(new { total = 0, rows = new List<NotifyRuleViewModel>()});
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
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
            Dictionary<string, byte[]> fileList = new Dictionary<string, byte[]>();
            var systemconfig = _settingService.GetConfigByKey(ConfigList.ConfigXMLPath);
            var config = XMLHelper.ConvertXMLEntity<ConfigSetting>(systemconfig.ConfigXML);
            try
            {
                FileHelper helpers = new FileHelper();
               
                var tmfilters = JsonConverter.Deserialize<TMFFilter>(tmfilter);
                var tmflist = _documentService.GetAllDocumentsByStudyId(CurUser.Id, tmfilters.Study);
                foreach(var tmf in tmflist){
                    byte[] temp;
                    var filename = tmf.DocumentName + tmf.VersionId
                  + Constant.Document_TypeSplitFlag + tmf.DocumentType;
                    string path = GetFilePath(config, tmf, filename);
                    temp= helpers. DownloadFile(path, config);
                    if (fileList.Any(f => f.Key == path))
                    {
                        continue;
                    }
                    else
                    {
                        fileList.Add(path, temp);
                    }
               
                }
                
                var targetPath=Server.MapPath("/TempFiles/"+tmfilters.StudyNum+".zip");
             
                  helpers.ZipFile(fileList, tmfilters.StudyNum, targetPath,config.HostType == (int)HostType.SharePoint);
               var temps = helpers.DownloadfromFileSystem(targetPath);
               var indexs=targetPath.Split('.');
               var files = targetPath.Split('\\');
               return File(temps, indexs[indexs.Length - 1], files[files.Length-1]);
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
            var systemconfig=_settingService.GetConfigByKey(ConfigList.ConfigXMLPath);
            var config = XMLHelper.ConvertXMLEntity<ConfigSetting>(systemconfig.ConfigXML);
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
        public JsonResult SaveNotifyRules(string notifyRules)
        {
         
            try 
            {
                if (!string.IsNullOrEmpty(notifyRules))
                { 
                    List<NotifyRuleViewModel> tmfs = JsonConverter.Deserialize<List<NotifyRuleViewModel>>(notifyRules);
                    _documentService.SaveNotifyRules(tmfs);
                }
                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                return Json(new {result = false,  ex = ex.Message });
            }

        }




        [LoginFilter]
        [HttpPost]
        public JsonResult DeleteNotifyRules(string notifyRules)
        {

            try
            {
                if (!string.IsNullOrEmpty(notifyRules))
                {
                    List<NotifyRuleViewModel> tmfs = JsonConverter.Deserialize<List<NotifyRuleViewModel>>(notifyRules);
                    _documentService.DeleteNotifyRules(tmfs);
                }
                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                return Json(new { result = false, ex = ex.Message });
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
                        _documentService.SaveDocument(tft, CurUser.Id, CurUser.UserName, false);
                    });

                }
                return Json(new { result = true });

            }
            catch (Exception ex)
            {
                return Json(new { result = false, ex = ex.Message });
            }

        }
        
    }
}