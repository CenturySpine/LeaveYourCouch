using System.Collections.Generic;

namespace LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi
{
    public class DirectionObject
    {
        public List<GeocodedWaypoint> geocoded_waypoints { get; set; }
        public List<Route> routes { get; set; }
        public string status { get; set; }
    }
}