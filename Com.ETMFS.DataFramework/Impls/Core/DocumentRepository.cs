using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.History;
using Com.ETMFS.DataFramework.Interfaces.Core;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Core
{
    public class DocumentRepository : Repository<StudyDocument, ETMFContext>, IDocumentRepository
    {
        public DocumentRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }


       public PageResult<StudyDocumentHistory> GetDocumentHistoryList(PageResult<StudyDocumentHistory> pagein, int id)
        {
            var document = Dbset.Find(id);
            pagein.Total = document.StudyDocumentHistory.Count;
            pagein.ResultRows = document.StudyDocumentHistory
                .OrderByDescending(f=>f.Modified)
                .Skip(pagein.SkipCount).Take(pagein.PageSize).ToList();
            return pagein;
        }

       public PageResult<DocumentView> GetDocumentViewList(PageResult<DocumentView> pagein,int p,TMFFilter condition){
           var temp = pagein;
           var list = LoadDocumentList(p,condition);
           temp.Total = list.Count;
           temp.ResultRows = list.Skip(pagein.SkipCount).Take(pagein.PageSize).ToList();
           return temp;
       }

    public   List<DocumentView> LoadDocumentList(int p, TMFFilter condition)
       {
              var list = DataContext.DocumentView.Where(f=>f.UploaderId==p
               &&(!condition.Country.HasValue||condition.Country==f.CountryId)
               && (!condition.Site.HasValue || condition.Site == f.SiteId)
               && (!condition.Study.HasValue || condition.Study == f.StudyId)
               && (!condition.TMFId.HasValue || condition.TMFId == f.StudyTemplateId)
               && (string.IsNullOrEmpty(condition.SectionNo) || condition.SectionNo == f.SectionNo)
               && (string.IsNullOrEmpty(condition.ZoneNo) || condition.ZoneNo == f.ZoneNo)
               && (string.IsNullOrEmpty(condition.ArticleNo) || f.ArtifactNo.Contains(condition.ArticleNo))
               && (string.IsNullOrEmpty(condition.DocumentName) || f.DocumentName.Contains(condition.DocumentName))
              && (string.IsNullOrEmpty(condition.ArticleName) || f.ArtifactName.Contains(condition.ArticleName))
               ).ToList();

              return list;
        }
    
    }
}
