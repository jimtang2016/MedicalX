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
 public   class CountryRepository : Repository<Country, ETMFContext>, ICountryRepository
    {
     public CountryRepository(IDataContextFactory<ETMFContext> databaseFactory)
            : base(
            databaseFactory) 
        {
            
        }


     #region ICountryRepository Members

     public PageResult<Country> GetCountries(PageResult<Country> page)
     {
         var pageresult = page;
         var list=Dbset.Where(f=>f.Active.HasValue&&f.Active.Value).ToList();
         pageresult.Total = list.Count;
         pageresult.ResultRows = list.Skip(pageresult.SkipCount).Take(pageresult.PageSize).ToList();
         return pageresult;
     }
     public List<Country> GetCountries()
     {
         var list = Dbset.Where(f => f.Active.HasValue && f.Active.Value).ToList();
         return list;
     }
     #endregion
    }
}
