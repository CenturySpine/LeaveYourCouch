using System.Collections.Generic;

namespace LeaveYourCouch.Mvc.GooglePlaceApiModels
{
    public class Prediction
    {
        public string description { get; set; }
        public string id { get; set; }
        public List<MatchedSubstring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public structured_formatting structured_formatting { get; set; }
        public List<term> terms { get; set; }
        public List<string> types { get; set; }
    }
}