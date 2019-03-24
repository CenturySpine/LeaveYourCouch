using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.Business.Services;
using LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi;
using Newtonsoft.Json;

namespace LeaveYourCouch.Mvc.Business
{
    public class ApiHelper : IApiHelper
    {


        public async Task<DirectionObject> GetDirections(string userAdress, string eventAddress, string unit, DirectionModes md1)
        {
            string apkey = SecretConfiguration.Get("google.direction.api");

            HttpClient httpcli = new HttpClient();
            var start = truncateaddress(userAdress);
            var end = truncateaddress(eventAddress);
            var request =
                $@"https://maps.googleapis.com/maps/api/directions/json?origin=" + start + "&destination=" + end + $"&mode={md1}&units={unit}&key={apkey}";

            var result = await httpcli.GetStringAsync(request);
            var objResult = JsonConvert.DeserializeObject<DirectionObject>(result);



            return objResult;
        }

        public string GenerateMapLink(string usrPostalCode, string targeteventAddress, DirectionModes mode)
        {
            var start = $"origin={truncateaddress(usrPostalCode)}";
            var end = $"destination={truncateaddress(targeteventAddress)}";
            var mod = $"travelmode={mode}";
            //https://www.google.com/maps/dir/?api=1&origin=Space+Needle+Seattle+WA&destination=Pike+Place+Market+Seattle+WA&travelmode=bicycling

            var result = $@"https://www.google.com/maps/dir/?api=1&" + start + "&" + end + "&" + mod;
            return result;
        }

        private string truncateaddress(string inputaddress)
        {
            return inputaddress.Replace(",", "")?.Replace(";", "").Replace("?", "").Replace(" ", "+");
        }
    }
}