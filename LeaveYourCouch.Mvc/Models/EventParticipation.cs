using System;

namespace LeaveYourCouch.Mvc.Models
{
    public class EventParticipation
    {
        public int Id { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
        public ParticipationStatus Status { get; set; }
        public DateTime SubscriptionTime { get; set; }

    }
}