using System.Net.Http;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.GooglePlaceApiModels;
using Newtonsoft.Json;

namespace LeaveYourCouch.Mvc.Business.Services
{
    internal class CityServices : ICityServices
    {
        public async Task<nearbyplaces> SearchCity(string input)
        {
            var ulrRequest = $@"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={input}&types=(regions)&language=fr&components=country:fr&radius=50000&key=AIzaSyDtDGjLnw-S73R1l2VcS-5mKZi42R9JXkE";
            HttpClient httpcli = new HttpClient();
            var result = await httpcli.GetStringAsync(ulrRequest);
            AutocompleteResult pocoResult = JsonConvert.DeserializeObject<AutocompleteResult>(result);

            if (pocoResult.predictions.Count > 0)
            {
                var primary = pocoResult.predictions[0];
                var searchEpicentreUrl = $@"https://maps.googleapis.com/maps/api/place/details/json?placeid={primary.place_id}&key=AIzaSyC6brf9Bm7PBSXGzDwg2_lD10c-JAMspHo";
                var jsonplace = await httpcli.GetStringAsync(searchEpicentreUrl);
                var poco = JsonConvert.DeserializeObject<PlacesResult>(jsonplace);
                //var detailedurl = $@"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={input}&types=(regions)&language=fr&components=country:fr&radius=10000&location={poco.result.geometry.location.lat},{poco.result.geometry.location.lng}&key=AIzaSyDtDGjLnw-S73R1l2VcS-5mKZi42R9JXkE";
                //result = await httpcli.GetStringAsync(detailedurl);
                //pocoResult = JsonConvert.DeserializeObject<AutocompleteResult>(result);

                var geocodeurl = $@"http://api.geonames.org/findNearbyPlaceNameJSON?lat={poco.result.geometry.location.lat}&lng={poco.result.geometry.location.lng}&style=short&cities=cities5000&radius=30&maxRows=30&username=centuryspine";
                var geocodenearby = await httpcli.GetStringAsync(geocodeurl);

                //return JsonConvert.DeserializeObject(geocodenearby);

                return
                    //Json(
        //public async Task<nearbyplaces> SearchCity(string input)
        JsonConvert.DeserializeObject<nearbyplaces>(geocodenearby);
                    //, JsonRequestBehavior.AllowGet)
                    ;

            }

            return
                new nearbyplaces()
                ;
        }
    }
}
