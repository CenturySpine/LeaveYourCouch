using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveYourCouch.Mvc.Models.Events
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "event_create_title", ResourceType = typeof(Resources.Resources))]
        public string Title { get; set; }

        [Display(Name = "event_create_description", ResourceType = typeof(Resources.Resources))]
        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "event_create_date", ResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "event_create_time", ResourceType = typeof(Resources.Resources))]
        public TimeSpan Time { get; set; }

        [Display(Name = "event_create_meeting_point", ResourceType = typeof(Resources.Resources))]
        public string MeetingPoint { get; set; }

        [Required]
        [Display(Name = "event_create_address", ResourceType = typeof(Resources.Resources))]
        public string Address { get; set; }

        [Required]
        [Display(Name = "event_create_participants", ResourceType = typeof(Resources.Resources))]
        public int MaxParticipants { get; set; }

        [Display(Name = "event_create_private", ResourceType = typeof(Resources.Resources))]
        public bool IsPrivate { get; set; }

        public ApplicationUser Owner { get; set; }
    }
}