using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using LeaveYourCouch.Models.GooglePlaceApiModels;
using LeaveYourCouch.Services;
using Newtonsoft.Json;

namespace LeaveYourCouch.Mvc.Controllers
{
    
    public class CitiesController : ApiController
    {
        [HttpGet]
        [Route("api/cities/search-nearby")]
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