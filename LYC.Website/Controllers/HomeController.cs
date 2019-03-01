using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using LYC.Website.Models;
using Newtonsoft.Json;

namespace LYC.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new HomeViewModel(User));
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
        public async Task<ActionResult> SearchCity(string input)
        {
            var ulrRequest = $@"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={input}&types=(regions)&language=fr&components=country:fr&radius=50000&key=AIzaSyDtDGjLnw-S73R1l2VcS-5mKZi42R9JXkE";
            HttpClient httpcli = new HttpClient();
            var result = await httpcli.GetStringAsync(ulrRequest);
            AutocompleteResult pocoResult = JsonConvert.DeserializeObject<AutocompleteResult>(result);

            if (pocoResult.predictions.Count > 0)
            {
                var primary = pocoResult.predictions[0];
                var searchEpicentreUrl= $@"https://maps.googleapis.com/maps/api/place/details/json?placeid={primary.place_id}&key=AIzaSyC6brf9Bm7PBSXGzDwg2_lD10c-JAMspHo";
                var jsonplace = await httpcli.GetStringAsync(searchEpicentreUrl);
                var poco = JsonConvert.DeserializeObject<PlacesResult>(jsonplace);
                //var detailedurl = $@"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={input}&types=(regions)&language=fr&components=country:fr&radius=10000&location={poco.result.geometry.location.lat},{poco.result.geometry.location.lng}&key=AIzaSyDtDGjLnw-S73R1l2VcS-5mKZi42R9JXkE";
                //result = await httpcli.GetStringAsync(detailedurl);
                //pocoResult = JsonConvert.DeserializeObject<AutocompleteResult>(result);

                var geocodeurl = $@"http://api.geonames.org/findNearbyPlaceNameJSON?lat={poco.result.geometry.location.lat}&lng={poco.result.geometry.location.lng}&style=short&cities=cities5000&radius=30&maxRows=30&username=centuryspine";
                var geocodenearby = await httpcli.GetStringAsync(geocodeurl);
                return Json(geocodenearby, JsonRequestBehavior.AllowGet);

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }

    class PlacesResult
    {
        public placedetailresult result { get; set; }
    }

    class placedetailresult
    {
        public geometry geometry { get; set; }

    }
    class geometry
    {
        public location location { get; set; }
    }
    class location
    {
        public string lat { get; set; }
        public string lng { get; set; }
    }

    class AutocompleteResult
    {
        public List<prediction> predictions { get; set; }

    }

    class prediction
    {
        public string description { get; set; }
        public string id { get; set; }
        public List<matched_substring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public structured_formatting structured_formatting { get; set; }
        public List<term> terms { get; set; }
        public List<string> types { get; set; }
    }

    class matched_substring
    {
        public string length { get; set; }
        public string offset { get; set; }
    }
    class term
    {
        
        public string offset { get; set; }
        public string value { get; set; }
    }
    class structured_formatting
    {
        public string main_text { get; set; }
        public List<matched_substring> main_text_matched_substrings { get; set; }
        public string secondary_text { get; set; }

    }
}


//{
//   "predictions" : [
//      {
//         "description" : "Roanne, France",
//         "id" : "f8d3cf7fd059e1d72f5984ee1b9b133e49359b9b",
//         "matched_substrings" : [
//            {
//               "length" : 6,
//               "offset" : 0
//            }
//         ],
//         "place_id" : "ChIJM_8Z6UAP9EcRQA3oy688CQQ",
//         "reference" : "CjQmAAAA3V_8aAgQEAld7y8vvWtzuWBti-8qkleRwE1tT_Nkg0XK_UnF9DU-sgEIcvwujNpKEhAp2XZM1Srxdkz9uWRpXEEzGhQZ3j4Wy1io_aH_KxCJeMIA5M8NbQ",
//         "structured_formatting" : {
//            "main_text" : "Roanne",
//            "main_text_matched_substrings" : [
//               {
//                  "length" : 6,
//                  "offset" : 0
//               }
//            ],
//            "secondary_text" : "France"
//         },
//         "terms" : [
//            {
//               "offset" : 0,
//               "value" : "Roanne"
//            },
//            {
//               "offset" : 8,
//               "value" : "France"
//            }
//         ],
//         "types" : [ "locality", "political", "geocode" ]
//      },
//      {
//         "description" : "Roannes-Saint-Mary, France",
//         "id" : "cd54ae3a0d082a888e5c7dfe8f3fc7dd53c7021f",
//         "matched_substrings" : [
//            {
//               "length" : 6,
//               "offset" : 0
//            }
//         ],
//         "place_id" : "ChIJNaTE6SVXrRIRJ5CddNfrVOc",
//         "reference" : "CkQyAAAAfS2LEawoMZ53MBIttx_FzY6hRW4aBlECynj42NH4KQaHD-jUmapEWFAtW8re-HzRjmJuFQiq1qkHbXEN0QWd-hIQqjUw1j8tu6kB6-lCPU5LHRoUArDoLix_InI8I1x3_hGhoQ-s6CE",
//         "structured_formatting" : {
//            "main_text" : "Roannes-Saint-Mary",
//            "main_text_matched_substrings" : [
//               {
//                  "length" : 6,
//                  "offset" : 0
//               }
//            ],
//            "secondary_text" : "France"
//         },
//         "terms" : [
//            {
//               "offset" : 0,
//               "value" : "Roannes-Saint-Mary"
//            },
//            {
//               "offset" : 20,
//               "value" : "France"
//            }
//         ],
//         "types" : [ "locality", "political", "geocode" ]
//      },
//      {
//         "description" : "Roanne, Dampniat, France",
//         "id" : "8e386510b2af9a0757e508a3b99df453e3f9fa89",
//         "matched_substrings" : [
//            {
//               "length" : 6,
//               "offset" : 0
//            }
//         ],
//         "place_id" : "ChIJ405znTiQ-EcRQvIu4nslFXk",
//         "reference" : "CjQwAAAAcAboRttzPPeFzy551pQDQ7G2MCm3s9v1UMKaBxXXnV0AVBOmjQ7DudellNk6krRREhA8xPlTs7Ypt6Dm4L61iC_YGhQkXNeDQYq0Fr7yBMvEk7_u0OawVA",
//         "structured_formatting" : {
//            "main_text" : "Roanne",
//            "main_text_matched_substrings" : [
//               {
//                  "length" : 6,
//                  "offset" : 0
//               }
//            ],
//            "secondary_text" : "Dampniat, France"
//         },
//         "terms" : [
//            {
//               "offset" : 0,
//               "value" : "Roanne"
//            },
//            {
//               "offset" : 8,
//               "value" : "Dampniat"
//            },
//            {
//               "offset" : 18,
//               "value" : "France"
//            }
//         ],
//         "types" : [ "sublocality_level_1", "sublocality", "political", "geocode" ]
//      }
//   ],
//   "status" : "OK"
//}


//https://maps.googleapis.com/maps/api/place/autocomplete/json?input=Vict&types=(cities)&language=pt_BR&key=YOUR_API_KEY