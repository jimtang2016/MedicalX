using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.Service.Core.ViewModel;
 
namespace Com.ETMFS.Service.Core.Interfaces
{
  public  interface IDocumentService
    {
      PageResult<DocumentViewModel> GetDocumentList(int page, int rows, int p,TMFFilter condition);

      void SaveDocument(TMFFilter tmf,  int userId,string userName);

      PageResult<DocumentViewModel> GetDocumentHistory(int page, int rows, int id);

      DocumentSumView GetUploadConculation(int p, TMFFilter condition);

      DocumentSumView GetReviewConculation(int p, TMFFilter condition);

      PageResult<IssueLogViewModel> GetIssueLogs(int page, int rows, int id,bool isAllIssues,string status);
    }
}
