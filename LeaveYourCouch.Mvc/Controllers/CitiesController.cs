using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using LeaveYourCouch.Mvc.Business.Services;
using LeaveYourCouch.Mvc.GooglePlaceApiModels;
using Newtonsoft.Json;

namespace LeaveYourCouch.Mvc.Controllers
{
    
    public class CitiesController : ApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/cities/search-nearby")]
        //[System.Web.Mvc.Route("api/searchnearby", Name = "citysearchnearby")]
        public async Task<IHttpActionResult> Search(string input)
        {
            try
            {
                var res = await Bootstrapper.CityService().SearchCity(input);
                return Json(res);
            }
            catch (Exception ex)
            {

                return Json(new nearbyplaces());
            }
        }
    }
}