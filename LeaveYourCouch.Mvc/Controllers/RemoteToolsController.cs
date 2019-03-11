using System;
using System.Threading.Tasks;
using System.Web.Http;
using LeaveYourCouch.Models.GooglePlaceApiModels;
using LeaveYourCouch.Services;

namespace LeaveYourCouch.Mvc.Controllers
{
    public class RemoteToolsController : ApiController
    {
        [HttpGet]
        [Route("api/tools/creatdb")]
        
        public async Task<IHttpActionResult> CreateDatabase()
        {
            SimpleLogger.Log("RemoteToolsController.CreateDatabase", "Creation DB request throught api");

            DbcontextTools.Create();
            return Json("Terminated. See logs for details");
        }
    }
}