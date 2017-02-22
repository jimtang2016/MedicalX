using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Entities.History;
using Com.ETMFS.DataFramework.View;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Core
{
    public interface IDocumentRepository : IRepository<StudyDocument>
    {
        PageResult<DocumentView> GetDocumentViewList(PageResult<DocumentView> pagein,int p,TMFFilter condition);

        PageResult<StudyDocumentHistory> GetDocumentHistoryList(PageResult<StudyDocumentHistory> pagein, int id);

        List<DocumentView> LoadDocumentList(int p, TMFFilter condition);

     
    }
}
