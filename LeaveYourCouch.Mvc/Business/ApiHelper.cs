using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.Business.Services;
using LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi;
using Newtonsoft.Json;

namespace LeaveYourCouch.Mvc.Business
{
    public enum DirectionModes
    {
        driving,
        walking,
        bicycling,
        transit
    }
    public class ApiHelper : IApiHelper
    {


        public async Task<Dictionary<DirectionModes, DirectionObject>> GetDirections(string userAdress, string eventAddress,
            string unit)
        {
            Dictionary<DirectionModes, DirectionObject> directions = new Dictionary<DirectionModes, DirectionObject>();
            string apkey = SecretConfiguration.Get("google.direction.api");
            foreach (var dmodes in Enum.GetNames(typeof(DirectionModes)))
            {
                HttpClient httpcli = new HttpClient();
                var start = truncateaddress(userAdress);
                var end = truncateaddress(eventAddress);
                var request =
                    $@"https://maps.googleapis.com/maps/api/directions/json?origin=" + start + "&destination=" + end + $"&mode={dmodes}&units={unit}&key={apkey}";

                var result = await httpcli.GetStringAsync(request);
                var objResult = JsonConvert.DeserializeObject<DirectionObject>(result);
                DirectionModes md = (DirectionModes)Enum.Parse(typeof(DirectionModes), dmodes);
                directions.Add(md, objResult);
            }


            return directions;
        }

        public string GenerateMapLink(string usrPostalCode, string targeteventAddress, DirectionModes mode)
        {
            //string encodedmode;
            //switch (mode)
            //{
            //    case DirectionModes.driving:
            //        encodedmode = "d";
            //        break;
            //    case DirectionModes.walking:
            //        encodedmode = "w";
            //        break;
            //    case DirectionModes.bicycling:
            //        encodedmode = "b";
            //        break;
            //    case DirectionModes.transit:
            //        encodedmode = "r";
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            //}
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

    public interface IApiHelper
    {
        Task<Dictionary<DirectionModes, DirectionObject>> GetDirections(string userAdress, string eventAddress, string unit);
        string GenerateMapLink(string usrPostalCode, string targeteventAddress, DirectionModes mode);
    }
}