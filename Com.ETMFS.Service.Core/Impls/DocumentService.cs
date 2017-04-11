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
        public PageResult<IssueLogViewModel> GetIssueLogs(int page, int rows, int id,bool isAllIssues,string status)
        {
            PageResult<IssueLog> pagein = new PageResult<IssueLog>()
            {
                PageSize = rows,
                CurrentPage = page
            };
            pagein = _documentRepos.GetDocumentIssueList(pagein, id, isAllIssues, status);
            PageResult<IssueLogViewModel> pageout = new PageResult<IssueLogViewModel>()
            {
                PageSize = rows,
                CurrentPage = page,
                Total = pagein.Total
            };
            pageout.ResultRows =pagein.ResultRows.Select(s=>ConvertIssuLogView(s)).ToList();
            return pageout;
        }

        IssueLogViewModel ConvertIssuLogView(IssueLog log)
        {

            IssueLogViewModel view = new IssueLogViewModel()
            {
                Id = log.Id,
                Active = log.Active,
                Comments = log.Comments,
                Reason = log.Reason,
                ReviewDate = log.ReviewDate.Value.ToString(Constant.Date_format),
                DocumentId = log.StudyDocument.Id,
                ReviewerId =log.Reviewer!=null? log.Reviewer.Id:0,
                ReviewName = log.Reviewer != null ? log.Reviewer.UserName : string.Empty,
                Status = log.Status,
                LogNum=log.LogNum
            };
           
            log.AssignedUsers.ToList().ForEach(u =>
            {
                if (u.AssignUser != null)
                {
                    if (u.IsOther != null && u.IsOther.Value)
                    {
                        if (string.IsNullOrEmpty(view.OthersUsers))
                        {
                            view.OthersUsers = u.AssignUser.UserName;
                        }
                        else
                        {
                            view.OthersUsers = view.OthersUsers + Constant.Document_UserFlag + u.AssignUser.UserName;
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(view.AssignUserIds))
                        {
                            view.AssignUserIds = u.AssignUser.Id.ToString();
                            view.AssignedUsers = u.AssignUser.UserName;
                        }
                        else
                        {
                            view.AssignUserIds = view.AssignUserIds + Constant.Document_UserFlag + u.AssignUser.Id.ToString();
                            view.AssignedUsers = view.AssignedUsers + Constant.Document_UserFlag + u.AssignUser.UserName;
                        }

                    }
                }
               
                
            });
            return view;
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
            if (tmf.Operation == OperationType.Update || tmf.Operation == OperationType.Create)
            {
                document.VersionId = tmf.VersionId;
                document.UploaderId = userId;
                document.ModifiBy = userName;
                document.Modified = DateTime.Now;
                document.DocumentName = tmf.DocumentName;
                document.DocumentType = tmf.DocumentType;
                document.HasIssue = tmf.HasIssue;
                document.IssueLogIds = tmf.IssueLogIds;
                document.SiteId = tmf.Site;
                document.IssueLoges = tmf.IssueLoges;
                document.DocumentLevel = tmf.DocumentLevel;
                document.ProtocolNumber = tmf.ProtocolNumber;
                document.Language = tmf.Language;
                document.IsCountryShared = tmf.IsCountryShared;
                document.IsSiteShared = tmf.IsSiteShared;
                document.SharedCountryIds = tmf.SharedCountryIds;
                document.SharedCountryNames = tmf.SharedCountryNames;
                document.SharedSiteIds = tmf.SharedSiteIds;
                document.SharedSiteNames = tmf.SharedSiteNames;
                document.CountryId = tmf.Country;
                document.DocumentDate = DateTime.Parse(tmf.DocumentDate);
                document.TMFType = tmf.TMFType;
                if (tmf.HasIssue.HasValue&&tmf.HasIssue.Value)
                {
                    if (!string.IsNullOrEmpty(tmf.IssueLogIds))
                    {
                        var issues = tmf.IssueLogIds.Split(Constant.Document_UserFlag);
                        foreach (var issue in issues)
                        {
                            var temp=document.IssueLogs.FirstOrDefault(f=>f.Id==int.Parse(issue));
                            if (temp != null)
                            {
                                temp.Status = Constant.TMF_Resolved;
                                temp.Comments = tmf.Comments;
                            }

                        }
                    }
                }
            }
            
            
            if (tmf.Operation == OperationType.Review)
            {
                document.Status = DocumentStatus.Reviewed;
            }
            else if (tmf.Operation == OperationType.Issued)
            {
                document.Status = DocumentStatus.Issued;
                var issue = ConvertIssueLog(tmf, userName);
                issue.StudyDocument = document;
                document.IssueLogs.Add(issue);
                var builder = new StringBuilder();
                issue.AssignedUsers.ToList().ForEach(user =>
                {
                    builder.Append(user.AssignUser.Email + Constant.Document_EmailFlag);
                });
         
                var body = string.Format(EmailHelper.Current.EmailConfig.IssueTemplate, tmf.IssueLogViewModel.Reason, tmf.IssueLogViewModel.ReviewDate, tmf.DocumentId);
                var receivers = builder.ToString();
                //EmailHelper.Current.SendEmail(receivers.Substring(0, receivers.Length - 1), string.Empty, body);
            }
            else if (tmf.Operation != OperationType.Delete)
            {
                document.Status = DocumentStatus.Uploaded;
            }
            document.Comments = tmf.Comments;
            document.Active = tmf.Active;
            if (document.Id <= 0)
            {
                _documentRepos.Add(document);
            }
            StudyDocumentHistory dochistory = new StudyDocumentHistory()
            {
                VersionId = document.VersionId,
                Active = document.Active,
                ModifiBy = userName,
                Modified = DateTime.Now,
                DocumentName = document.DocumentName,
                DocumentType = document.DocumentType,
                SiteId = document.SiteId,
                CountryId = document.CountryId,
                UploaderId=userId,
                Comments = document.Comments,
                CreateBy = userName,
                Created = DateTime.Now,
                Operation=tmf.Operation
                  
            };
            dochistory.Status = document.Status;
            dochistory.DocumentLevel = document.DocumentLevel;
            dochistory.ProtocolNumber = document.ProtocolNumber;
            dochistory.Language = document.Language;
            dochistory.IsCountryShared = document.IsCountryShared;
            dochistory.IsSiteShared = document.IsSiteShared;
            dochistory.SharedCountryIds = document.SharedCountryIds;

            dochistory.SharedCountryNames = document.SharedCountryNames;
            dochistory.SharedSiteIds = document.SharedSiteIds;
            dochistory.SharedSiteNames = document.SharedSiteNames;
            dochistory.DocumentDate = document.DocumentDate;
            document.StudyDocumentHistory.Add(dochistory);
            _unitwork.Commit();
        }

    public IssueLog ConvertIssueLog(TMFFilter tmf, string userName)
    {
        IssueLog issue = new IssueLog()
        {

            LogNum =Constant.Document_LogPrefix + DateTime.Now.Ticks,
            ReviewerName = tmf.IssueLogViewModel.ReviewName,
            Created = DateTime.Now,
            CreateBy = userName,
            ModifiBy = userName,
            Modified = DateTime.Now,
            Status= tmf.IssueLogViewModel.Status,
            Active = true,
            Comments = tmf.IssueLogViewModel.Comments,
            Reason = tmf.IssueLogViewModel.Reason,
            ReviewDate = DateTime.Parse(tmf.IssueLogViewModel.ReviewDate)
        };
        issue.Reviewer = _userRepos.GetById(tmf.IssueLogViewModel.ReviewerId);
        var tempusers = new StringBuilder();
        if (!string.IsNullOrEmpty(tmf.IssueLogViewModel.AssignUserIds))
        {
            tempusers.Append(tmf.IssueLogViewModel.AssignUserIds);
        }
        if (!string.IsNullOrEmpty(tmf.IssueLogViewModel.OthersUsers))
        {
            if (tempusers.Length > 0)
            {
                tempusers.Append(Constant.Document_UserFlag + tmf.IssueLogViewModel.AssignUserIds);
            }
            else
            {
                tempusers.Append(tmf.IssueLogViewModel.AssignUserIds);
            }

        }
        var users = tempusers.ToString().Split(Constant.Document_UserFlag);
        foreach (var item in users)
        {
            var signuser = _userRepos.GetAll().FirstOrDefault(u => u.Active.Value && (u.UserName == item || u.Id == int.Parse(item) || u.Email == item));
            if (signuser != null)
            {
                var user = new AssignedUser()
                {
                    AssignUser = signuser,
                    IssueLog = issue,
                    IsOther = !string.IsNullOrEmpty(tmf.IssueLogViewModel.OthersUsers) && tmf.IssueLogViewModel.OthersUsers.Contains(item)
                };
                issue.AssignedUsers.Add(user);
            }
            else
            {
                throw new Exception("Can't find the AssignUser :" + item);
            }

        }

        return issue;
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
            Done = list.Count(f => f.Status == Constant.TMF_Reviewed)
        };
        return view;
    }

    public DocumentSumView GetIssuedConculation(int p, TMFFilter condition)
    {
        var list = _documentRepos.LoadDocumentList(p, condition);
        DocumentSumView view = new DocumentSumView()
        {
            Total = list.Count,
            Done = list.Count(f => f.Status == Constant.TMF_Issued)
        };
        return view;
    }

    #endregion
    }
}
