using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;
using LeaveYourCouch.Mvc.Business.Services;
using LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LeaveYourCouch.UnitTests
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void TestGoogleApiCitySearchSequence()
        {

            CityServices hc = new CityServices();

            //nearbyplaces result = hc.SearchCity("Lyon").Result;

            //Assert.IsInstanceOfType(result, typeof(JsonResult));
            //JsonResult routeResult = result as JsonResult;

            //Assert.IsTrue(result != null );
            //Assert.IsTrue(result.geonames.Count >0);
        }
        [Ignore]
        [TestMethod]
        public void TestDurationRoutesApi()
        {
            //api key =DIRECTION => AIzaSyDmuMl8nM82CswxhQdVM31zpcM0dCSfVHQ

            //string startAdress = "14 rue godefroy 69006 Lyon";
            string startAdress = "69006 Lyon";
            string endAddress = "La triche 69001 Lyon";
            var ulrRequest = $@"https://maps.googleapis.com/maps/api/directions/json?origin={startAdress.Replace(" ", "+")}&destination={endAddress.Replace(" ", "+")}&mode=driving&units=metric&key={SecretConfiguration.Get("google.direction.api")}";
            HttpClient httpcli = new HttpClient();
            var result = httpcli.GetStringAsync(ulrRequest).Result;
            var objResult = JsonConvert.DeserializeObject<DirectionObject>(result);
        }
    }

    
}
