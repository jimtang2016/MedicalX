using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Areas.MasterData.Controllers
{
    public class StudyDocumentController : BaseController
    {
        IDocumentService _documentService;
        public StudyDocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }
        // GET: MasterData/StudyDocument
        [LoginFilter]
        public ActionResult Index()
        {
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

            var path = config.PathURI + splitflag + config.RootFolder + splitflag + tmf.StudyNum;

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
            var config = ControllerContext.HttpContext.Application["ConfigSetting"] as ConfigSetting;
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
                        var splitflag =Constant.Document_TypeSplitFlag;
                        var lastindex = file.FileName.LastIndexOf(splitflag);
                        tmf.DocumentName = file.FileName.Substring(0, lastindex);
                        tmf.DocumentType = file.FileName.Substring(lastindex + 1, file.FileName.Length - lastindex - 1);
                        var filename = tmf.DocumentName + tmf.VersionId
                            + splitflag + tmf.DocumentType;
                        SaveDocument(tmf, filename,file);
                    }
                  
                    _documentService.SaveDocument(  tmf,  CurUser.Id,CurUser.UserName );
                }
                return Json(new { result = true  });

            }
            catch (Exception ex)
            {
                return Json(new { ex = ex.ToString() });
            }

        }

        [LoginFilter]
        public FileResult DownloadFile(string tmfilter)
        {
            var config = ControllerContext.HttpContext.Application["ConfigSetting"] as ConfigSetting;
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
                throw (ex);
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
        public JsonResult SaveDocument(string tmf)
        {
         
            try 
            {
                if (tmf != null)
                {
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
                return Json(new { ex = ex.ToString() });
            }

        }
        
    }
}