using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveYourCouch.Mvc.Models.Events
{
    public class EventListItem
    {
        public int EventId { get; set; }

        [Display(Name = "event_index_tabular_title", ResourceType = typeof(Resources.Resources))]
        public string Title { get; set; }

        public bool IsPrivate { get; set; }
        public int MaxParticipants { get; set; }

        [Display(Name = "event_index_tabular_date", ResourceType = typeof(Resources.Resources))]
        public DateTime Date { get; set; }

        [Display(Name = "event_index_tabular_time", ResourceType = typeof(Resources.Resources))]
        public TimeSpan Time { get; set; }

        [Display(Name = "event_index_tabular_participants", ResourceType = typeof(Resources.Resources))]
        public int Participants { get; set; }

        [Display(Name = "event_index_tabular_owner", ResourceType = typeof(Resources.Resources))]
        public string Owner { get; set; }

        public string OwnerId { get; set; }
    }
}