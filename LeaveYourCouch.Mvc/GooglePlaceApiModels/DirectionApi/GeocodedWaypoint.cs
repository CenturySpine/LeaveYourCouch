using System.Collections.Generic;

namespace LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi
{
    public class GeocodedWaypoint
    {
        public string geocoder_status { get; set; }
        public string place_id { get; set; }
        public List<string> types { get; set; }
    }
}
