using System;
using System.Web.Mvc;

using LeaveYourCouch.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeaveYourCouch.UnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void TestGoogleApiCitySearchSequence()
        {

            CityServices hc = new CityServices();

            var result = hc.SearchCity("Lyon").Result;

            //Assert.IsInstanceOfType(result, typeof(JsonResult));
            //JsonResult routeResult = result as JsonResult;

            Assert.IsTrue(result != null );
        }
    }
}
