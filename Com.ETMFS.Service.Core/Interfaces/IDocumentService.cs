using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.View;
using Com.ETMFS.Service.Core.ViewModel;
 
namespace Com.ETMFS.Service.Core.Interfaces
{
 public   enum TopType{
     ReuploadCount

    }
  public  interface IDocumentService
    {
      PageResult<DocumentViewModel> GetDocumentList(int page, int rows, int p,TMFFilter condition);

      void SaveDocument(TMFFilter tmf, int userId, string userName, bool hasFile);

 

      PageResult<DocumentViewModel> GetDocumentHistory(int page, int rows, int id);

      DocumentSumView GetUploadConculation(int p, TMFFilter condition);

      DocumentSumView GetReviewConculation(int p, TMFFilter condition);
      DocumentSumView GetIssuedConculation(int p, TMFFilter condition);

      List<DocumentSumView> GetTopTmfDocuments(int p, TMFFilter condition,int range,TopType topType );

      PageResult<IssueLogViewModel> GetIssueLogs(int page, int rows, int id,bool isAllIssues,string status);

      PageResult<NotifyRuleViewModel> GetNotifyRules(int? studyId, int? countryId, int? siteId,int? tmfId, int page, int rows);

      void DeleteNotifyRules(List<NotifyRuleViewModel> rulelist);

      void SaveNotifyRules(List<NotifyRuleViewModel> rulelist);

      List<NotifyReminderView> GetNotifyReminderList();

      List<DocumentViewModel> GetAllDocumentsByStudyId(int p,int? studyId);
    }
}
