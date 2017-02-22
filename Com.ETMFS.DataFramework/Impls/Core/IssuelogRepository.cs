using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.DataFramework.Interfaces.Core;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Core
{
    public class IssuelogRepository : Repository<IssueLog, ETMFContext>, IIssuelogRepository
    {
        public IssuelogRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }
    }
}
