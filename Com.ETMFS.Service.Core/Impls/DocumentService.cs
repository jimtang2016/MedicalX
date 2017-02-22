using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.History;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework.Interfaces.Permission;
using Com.ETMFS.Service.Common;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.Service.Core.Impls
{
    struct DocumentStatus
    {
        internal static string Uploaded = "Uploaded";
        internal static string Reviewed = "Reviewed";
        internal static string Issued = "Issued";
    }

    struct OperationType
   {
       internal static string Create = "Create";
       internal static string Issued = "Issued";
       internal static string Review = "Review";
       internal static string Update = "Update";
       internal static string Delete = "Delete";
   }
    public class DocumentService : IDocumentService
    {
        IDocumentRepository _documentRepos;
        IUnitOfWork _unitwork;
        IStudyRepository _studyRepos;
        IUsersRepository _userRepos;
        ITMFTemplateRepository _tmtcontext;
        public DocumentService(IDocumentRepository documentRepos, IUsersRepository userRepos, IUnitOfWork unitwork, IStudyRepository studyRepos, ITMFTemplateRepository tmtcontext)
        {
            _documentRepos = documentRepos;
            _unitwork = unitwork;
            _studyRepos = studyRepos;
            _tmtcontext = tmtcontext;
            _userRepos = userRepos;
        }
        public PageResult<DocumentViewModel> GetDocumentList(int page, int rows, int p, TMFFilter condition)
        {
            PageResult<DocumentView> pagein = new PageResult<DocumentView>()
            {
                PageSize = rows,
                CurrentPage = page
            };
            pagein = _documentRepos.GetDocumentViewList(pagein, p, condition);
            PageResult<DocumentViewModel> pageout = new PageResult<DocumentViewModel>()
            {
                PageSize = rows,
                CurrentPage = page,
                Total=pagein.Total
            };
           pageout.ResultRows= Common.Common<DocumentView, DocumentViewModel>.ConvertToViewModel(pagein.ResultRows);
           return pageout;
        }


        public PageResult<DocumentViewModel> GetDocumentHistory(int page, int rows, int id)
        {
            PageResult<StudyDocumentHistory> pagein = new PageResult<StudyDocumentHistory>()
            {
                PageSize = rows,
                CurrentPage = page
            };
            pagein = _documentRepos.GetDocumentHistoryList(pagein, id);
            PageResult<DocumentViewModel> pageout = new PageResult<DocumentViewModel>()
            {
                PageSize = rows,
                CurrentPage = page,
                Total = pagein.Total
            };
            pageout.ResultRows = Common.Common<StudyDocumentHistory, DocumentViewModel>.ConvertToViewModel(pagein.ResultRows);
            return pageout;
        }

    public  void SaveDocument(TMFFilter tmf, int userId, string userName)
        {
            var document = _documentRepos.GetById(tmf.DocumentId);
            if (document == null)
            {
                document = new StudyDocument()
                {
                    CreateBy = userName,
                    Created = DateTime.Now
                };
                var study = _studyRepos.GetById(tmf.Study.Value);

                if (study != null)
                {
                    var sttemp = study.StudyTemplate.FirstOrDefault(f => f.Id == tmf.StudyTemplateId || f.TMFTemplate.Id == tmf.TMFId);
                    document.StudyTemplate = sttemp;
                }

                
            }
            document.VersionId = tmf.VersionId;
            document.UploaderId = userId;
            document.ModifiBy = userName;
            document.Modified = DateTime.Now;
            document.DocumentName = tmf.DocumentName;
            document.DocumentType = tmf.DocumentType;
            document.SiteId = tmf.Site;
            if (tmf.Operation == OperationType.Review)
            {
                document.Status = DocumentStatus.Reviewed;
            }
            else if (tmf.Operation == OperationType.Issued)
            {
                document.Status = DocumentStatus.Issued;
                IssueLog issue=new IssueLog(){
                    DocumentId=document.Id,
                    ReviewerId=tmf.IssueLogViewModel.ReviewerId,
                     ReviewerName=userName,
                      Created=DateTime.Now,
                      CreateBy=userName,
                      ModifiBy=userName,
                      Modified=DateTime.Now,
                      Active=true,
                      Comments=tmf.IssueLogViewModel.Comments,
                    ReasonId = tmf.IssueLogViewModel.ReasonId,
                    ReviewDate =DateTime.Parse( tmf.IssueLogViewModel.ReviewDate)
                };
               var users= tmf.IssueLogViewModel.AssignedUsers.Split(',');
               foreach (var item in users)
               {
                   var user = new AssignedUser()
                   {
                       AssignUser = _userRepos.GetById(item),
                       IssueLog = issue
                   };
                   issue.AssignedUsers.Add(user);
               }
                document.IssueLogs.Add(issue);
            }else if(tmf.Operation!= OperationType.Delete){
                document.Status = DocumentStatus.Uploaded;
            }
           
           
            document.CountryId = tmf.Country;
            document.Comments = tmf.Comments;
            document.Active = tmf.Active;
            if (document.Id <= 0)
            {
                _documentRepos.Add(document);
            }
            StudyDocumentHistory dochistory = new StudyDocumentHistory()
            {
                VersionId = tmf.VersionId,
                Active = tmf.Active,
                ModifiBy = userName,
                Modified = DateTime.Now,
                DocumentName = tmf.DocumentName,
                DocumentType = tmf.DocumentType,
                SiteId = tmf.Site,
                CountryId = tmf.Country,
                UploaderId=userId,
                Comments = tmf.Comments,
                CreateBy = userName,
                Created = DateTime.Now,
                Operation=tmf.Operation
            };
            document.StudyDocumentHistory.Add(dochistory);
            _unitwork.Commit();
        }

    #region IDocumentService Members


    public DocumentSumView GetUploadConculation(int p, TMFFilter condition)
    {
        var uploadCount = _documentRepos.LoadDocumentList(p, condition);
        int count = 0;

     
        if (condition.Study != null)
        {
            if (condition.Study.HasValue)
            {

                var studylist = _studyRepos.GetUserStudyList(p).FirstOrDefault(from => from.Id == condition.Study);
                var siteCount = studylist.StudySite.Where(f => f.Site.Active.HasValue
                && f.Site.Active.Value).ToList();
                var countryCount = studylist.TrialRegional.Where(f => f.Active.HasValue
                 && f.Active.Value).ToList();

                var scount = siteCount.Count;
                var ccount = countryCount.Count;
                var stcount = 1;

                condition.TMFLevel = Constant.RoleLevel_Trial;

                if (condition.Country.HasValue)
                {
                    condition.TMFLevel = Constant.RoleLevel_Country;
                    scount = siteCount.Count(fc => fc.CountryId == condition.Country);
                    ccount = 1;
                    stcount = 0;
                }
                if (condition.Site.HasValue)
                {
                    condition.TMFLevel = Constant.RoleLevel_Site;
                    scount = 1;
                    ccount = 0;
                    stcount = 0;
                }

                var tmfsquery = _studyRepos.GetSutdyTemplates(studylist, condition);
                tmfsquery.ForEach(f =>
                {

                    count += GetDocumentCount(f.TMFTemplate, stcount, ccount, scount);
                });
            }
        }

        DocumentSumView view = new DocumentSumView()
        {
            Total = count,
            Done = uploadCount.Count(f => f.UploaderId.HasValue)
        };
        return view;
    }

    int GetDocumentCount(TMFTemplate tmf,int studycount,int countries,int sites)
    {
        var count = studycount;
        if (tmf.IsSiteLevel == Constant.TMF_BelongFlag)
        {
            count = count + sites;
        }
         if (tmf.IsCountryLevel == Constant.TMF_BelongFlag)
        {
            count = count + countries;
        }
        return count;
    }

    public DocumentSumView GetReviewConculation(int p, TMFFilter condition)
    {
        var list = _documentRepos.LoadDocumentList(p, condition);
        DocumentSumView view = new DocumentSumView()
        {
            Total = list.Count,
            Done = list.Count(f => f.ReviewId.HasValue)
        };
        return view;
    }

    #endregion
    }
}
