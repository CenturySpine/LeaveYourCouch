using LeaveYourCouch.Mvc.Business;

namespace LeaveYourCouch.Mvc.Models
{
    public class EventRelativeToUserInformation
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
        public string  Unit { get; set; }
        public double Distance  { get; set; }
        public double Duration { get; set; }
        public DirectionModes DirectionMode { get; set; }
        public string MapLink { get; set; }
    }
}