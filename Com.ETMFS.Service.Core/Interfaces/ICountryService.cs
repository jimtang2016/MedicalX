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
  public  interface ICountryService
    {
       PageResult<CountryViewModel> GetCountries(int page,int rows);

       void SaveCountry(CountryViewModel country, string operatorId);

       void DeleteCountry(List<CountryViewModel> list, string operatorId);

       List<CountryViewModel> GetCountries();

       
    }
}
