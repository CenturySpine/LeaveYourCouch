using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveYourCouch.Models.GooglePlaceApiModels
{
    public class geoname
    {
        public string admincode1 { get; set; }
        public string lng { get; set; }
        public string distance { get; set; }
        public string geonameid { get; set; }
        public string toponymname { get; set; }
        public string countryid { get; set; }
        public string fcl { get; set; }
        public string population { get; set; }
        public string countrycode { get; set; }
        public string name { get; set; }
        public string fclname { get; set; }
        public string countryName { get; set; }
        public string fcodeName { get; set; }
        public string adminName1 { get; set; }
        public string lat { get; set; }
        public string fcode { get; set; }
        
    }

    public class geonames : List<geoname>
    {

    }
}
