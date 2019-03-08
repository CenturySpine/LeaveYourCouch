using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeaveYourCouch.Services;
using LeaveYourCouch.Models.GooglePlaceApiModels;
namespace LeaveYourCouch.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        
        public async Task<ActionResult> Search(string input)
        {
            try
            {
                var res = await Bootstrapper.CityService().SearchCity(input);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json("no data");
            }
        }



    }
}