using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities;
using Com.ETMFS.DataFramework.Entities.Permission;
using Com.ETMFS.DataFramework.Interfaces.Permission;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Impls.Permission
{
   public class LoginHistoryRepository:Repository<LoginHistory,ETMFContext> ,ILoginHistoryRepository
    {
        public LoginHistoryRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }

   
    }
}
