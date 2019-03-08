using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using LeaveYourCouch.Models.GooglePlaceApiModels;
using LeaveYourCouch.Services;

namespace LeaveYourCouch.Api.Controllers
{
    [System.Web.Http.Authorize]
    public class CitiesController : ApiController
    {
        private readonly ICityServices _cityService;

        public CitiesController(ICityServices cityService)
        {
            _cityService = cityService;
        }
        public async Task<IHttpActionResult> SearchCities(string cityInput)
        {
            
            return Json(await Bootstrapper.CityService().SearchCity(cityInput));
        }
    }
}
