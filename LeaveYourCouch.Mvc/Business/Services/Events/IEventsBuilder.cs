﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.Models;
using LeaveYourCouch.Mvc.Models.Events;

namespace LeaveYourCouch.Mvc.Business.Services.Events
{
    public interface IEventsBuilder
    {
        Task AddParticipation(Event @event);

        Task CreateNewEvent(CreateEventViewModel @event);

        Task EditEvent(CreateEventViewModel @event);

        Task<EventDataDetailsViewModel> GetEventInfos(Event targetEvent);
        Task RemoveParticipation(Event @event);
        Task<List<EventListItem>> ListEvents();
        Task ConfirmEventDeletionAsync(Event id);
    }
}