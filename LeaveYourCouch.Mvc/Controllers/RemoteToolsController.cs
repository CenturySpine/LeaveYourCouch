using System;
using System.Threading.Tasks;
using System.Web.Http;
using LeaveYourCouch.Models.GooglePlaceApiModels;
using LeaveYourCouch.Services;

namespace LeaveYourCouch.Mvc.Controllers
{
    [Authorize]
    public class RemoteToolsController : ApiController
    {
        public static string _language = "en-US";
        [HttpGet]
        [Route("api/tools/creatdb")]
        
        public async Task<IHttpActionResult> CreateDatabase()
        {
            SimpleLogger.Log("RemoteToolsController.CreateDatabase", "Creation DB request throught api");

            DbcontextTools.Create();
            return Json("Terminated. See logs for details");
        }

        [HttpPost]
        [Route("api/tools/setculture/{lng}")]

        public async Task<IHttpActionResult> SetCulture(string lng)
        {
            _language = lng;
            return Json("Terminated. See logs for details");
        }

        [HttpGet]
        [Route("api/tools/dummy")]

        public async Task<IHttpActionResult> DumyAction()
        {
            

            
            return Json("Dummyaction executed");
        }
    }
}