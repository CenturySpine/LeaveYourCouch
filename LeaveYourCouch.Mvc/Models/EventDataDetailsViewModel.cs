using System.Collections.Generic;

namespace LeaveYourCouch.Mvc.Models
{
    public class EventDataDetailsViewModel
    {
        public Event Event { get; set; }
        public List<EventRelativeToUserInformation> UserData { get; set; }
        public bool CanModify { get; set; }
        public List<EventParticipation> Participants { get; set; }
    }
}