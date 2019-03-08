using System;
using System.Web.Mvc;
using LeaveYourCouch.Api.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeaveYourCouch.UnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void TestGoogleApiCitySearchSequence()
        {

            HomeController hc = new HomeController();

            var result = hc.SearchCity("Lyon").Result;

            Assert.IsInstanceOfType(result, typeof(JsonResult));
            JsonResult routeResult = result as JsonResult;
            
            Assert.IsTrue(routeResult.Data.ToString().Contains("Villeurbanne"));
            Assert.IsTrue(routeResult.Data.ToString().Contains("Tassin"));
        }
    }
}
