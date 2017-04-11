using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.DataFramework.Entities.Core;
using RepositoryT.Infrastructure;

namespace Com.ETMFS.DataFramework.Interfaces.Core
{
    public interface ICountryRepository : IRepository<Country>
    {
       
          PageResult<Country> GetCountries(PageResult<Country> page);

          List<Country> GetCountries();
    }
}
