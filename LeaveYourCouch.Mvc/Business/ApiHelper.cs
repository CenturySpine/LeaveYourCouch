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
                var request =
                    $@"https://maps.googleapis.com/maps/api/directions/json?origin={userAdress.Replace(" ", "+")}&destination={eventAddress.Replace(" ", "+")}&mode={dmodes}&units={unit}&key={apkey}";

                var result = await httpcli.GetStringAsync(request);
                var objResult = JsonConvert.DeserializeObject<DirectionObject>(result);
                DirectionModes md = (DirectionModes) Enum.Parse(typeof(DirectionModes), dmodes);
                directions.Add(md, objResult);
            }


            return directions;
        }
    }

    public interface IApiHelper
    {
        Task<Dictionary<DirectionModes, DirectionObject>> GetDirections(string userAdress, string eventAddress, string unit);
    }
}