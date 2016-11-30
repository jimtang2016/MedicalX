using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.DataFramework.Interfaces.Core;
using Com.ETMFS.DataFramework;
using Com.ETMFS.DataFramework.Entities.Core;
using Com.ETMFS.Service.Core.ViewModel;
using Com.ETMFS.Service.Common;
using RepositoryT.Infrastructure;
namespace Com.ETMFS.Service.Core.Impls
{
  public class CountryService:ICountryService
    {
      ICountryRepository _countrycontext;
      IUnitOfWork _unitwork;
      public CountryService(ICountryRepository countrycontext,IUnitOfWork unitwork )
      {
          _countrycontext = countrycontext;
          _unitwork = unitwork;
      }


      #region ICountryService Members
      public List<CountryViewModel> GetCountries()
      {
          var outs = _countrycontext.GetCountries();
         return Common<Country, CountryViewModel>.ConvertToViewModel(outs);
      }
      public DataFramework.PageResult<ViewModel.CountryViewModel> GetCountries(int page,int rows)
      {
          var pagein = new PageResult<Country>()
          {
              CurrentPage = page,
              PageSize = rows
          };
          var outs = _countrycontext.GetCountries(pagein);
          PageResult<CountryViewModel> result = new PageResult<CountryViewModel>()
          {
              CurrentPage = page,
              PageSize = rows,
              Total = outs.Total
          };
          result.ResultRows = Common<Country, CountryViewModel>.ConvertToViewModel(outs.ResultRows);
          return result;
      }

    
      public void SaveCountry(CountryViewModel country,string operatorId)
      {
          Country ecountry = _countrycontext.GetById(country.Id);
          if(ecountry==null){
              ecountry = new Country()
              {

                  Active = true,
                  Created = DateTime.Now,
                  CreatedBy = operatorId,
              };
              
          }
          ecountry.CountryCode = country.CountryCode;
          ecountry.CountryName = country.CountryName;
          ecountry.Modified = DateTime.Now;
          ecountry.ModifiedBy = operatorId;
          if (ecountry.Id == 0)
          {
              _countrycontext.Add(ecountry);
          }
          _unitwork.Commit();
      }

      public void DeleteCountry(List<CountryViewModel> countries, string operatorId)
      {
          countries.ForEach(f =>
          {
              var count = _countrycontext.GetById(f.Id);
              if (count != null)
              {
                  count.Active = false;
                  count.Modified = DateTime.Now;
                  count.ModifiedBy = operatorId;
              }
          });
          _unitwork.Commit();
      }
      #endregion
    }
}
