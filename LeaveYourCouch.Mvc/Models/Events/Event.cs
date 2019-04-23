using System;

namespace LeaveYourCouch.Mvc.Models.Events
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ApplicationUser Owner { get; set; }
        public int MaxSeats { get; set; }
        
        public string Address { get; set; }
        public string MeetingDetails { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        //public EventRelativeToUserInformation UserSpecificData { get; set; }
    }
}