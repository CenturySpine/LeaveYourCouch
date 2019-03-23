using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveYourCouch.Mvc.Models
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

        public string MeetingPoint { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int MaxParticipants { get; set; }

        public bool IsPrivate  { get; set; }

        public ApplicationUser Owner { get; set; }
    }

    public class EventsListViewModel
    {
        public List<Event> Events { get; set; }
    }

    public class EventDataDetailsViewModel
    {
        public Event Event { get; set; }
        public List<EventRelativeToUserInformation> UserData { get; set; }
        public bool CanModify { get; set; }
        public List<ApplicationUser> Participants { get; set; }
    }

    public class EventListItem
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public bool IsPrivate { get; set; }
        public int MaxParticipants { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int Participants { get; set; }
    }
}