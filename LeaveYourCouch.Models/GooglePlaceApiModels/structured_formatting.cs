using System.Collections.Generic;

namespace LeaveYourCouch.Models.GooglePlaceApiModels
{
    public class structured_formatting
    {
        public string main_text { get; set; }
        public List<MatchedSubstring> main_text_matched_substrings { get; set; }
        public string secondary_text { get; set; }

    }
}
