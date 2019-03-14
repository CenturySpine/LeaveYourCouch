using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using LeaveYourCouch.Models.GooglePlaceApiModels;
using LeaveYourCouch.Mvc.Models;
using LeaveYourCouch.Services;
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

    public class UsersController : Controller
    {
        [System.Web.Http.HttpPost]
        public JsonResult doesUserNameExist(string Pseudo)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u=>u.Pseudo== Pseudo);

                return Json(user == null);
            }

        }

        [System.Web.Http.HttpPost]
        public JsonResult doesUserEmailExist(string Email)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Email == Email);

                return Json(user == null);
            }

        }
    }

}