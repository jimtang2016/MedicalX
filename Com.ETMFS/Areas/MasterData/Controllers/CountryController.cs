using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.ETMFS.Service.Core.Interfaces;
using Com.ETMFS.Service.Core.ViewModel;

namespace Com.ETMFS.Areas.MasterData.Controllers
{
    public class CountryController :BaseController
    {
        ICountryService _countrycontext;
        public CountryController(ICountryService countrycontext)
        {
            _countrycontext = countrycontext;
        }
        // GET: MasterData/Country
        [LoginFilter]
        public ActionResult Index()
        {
            return View();
        }
        [LoginFilter]
        [HttpPost]
        public JsonResult GetCountryList(int page, int rows)
        {
            var users = _countrycontext.GetCountries(page, rows);
            return Json(new { total = users.Total, rows = users.ResultRows });

        }
        [LoginFilter]
        [HttpPost]
        public JsonResult SaveCountry(CountryViewModel country)
        {
             _countrycontext.SaveCountry(country,CurUser.UserName);
            return Json(new { result = true});

        }


        [LoginFilter]
        [HttpPost]
        public JsonResult DeleteCountrys(string countries)
        {
            var list = JsonConverter.Deserialize<List<CountryViewModel>>(countries);
            _countrycontext.DeleteCountry(list, CurUser.UserName);
            return Json(new { result = true });

        }
    }
}