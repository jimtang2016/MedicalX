using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Permission
{
    public class CompanyRepository :Repository<Company,ETMFContext> ,ICompanyRepository
    {
        public CompanyRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }

  
    }
}
