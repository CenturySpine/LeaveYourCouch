using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi;
using LeaveYourCouch.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveYourCouch.Mvc.Business.Services.Events
{
    public class EventsBuilder : IEventsBuilder
    {
        private readonly IApiHelper _apiHelper;
        private readonly ApplicationDbContext _db;

        public EventsBuilder(ApplicationDbContext db, IApiHelper apiHelper)
        {
            _db = db;
            _apiHelper = apiHelper;
        }

        public async Task AddParticipation(Event @event)
        {
            var usr = await GetCurrentUser();
            EventParticipation evtPart = new EventParticipation
            {
                Event = @event,
                SubscriptionTime = DateTime.Now,
                Status = ParticipationStatus.Confirmed,
                User = usr
            };
            _db.Participations.Add(evtPart);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveParticipation(Event @event)
        {
            var usr = await GetCurrentUser();
            var part = await _db.Participations.Include(p => p.User).FirstOrDefaultAsync(p => p.User.Id == usr.Id);


            if (part != null)
            {
                _db.Participations.Remove(part);
                await _db.SaveChangesAsync();
            }

        }

        public async Task<List<EventListItem>> ListEvents()
        {
            List<EventListItem> list = new List<EventListItem>();
            var allevents = await _db.Events.Include(p=>p.Owner).ToListAsync();
            foreach (var evt in allevents)
            {
                var parts = await _db.Participations.Include(p => p.Event).Where(f => f.Event.Id == evt.Id).CountAsync();
                var display = new EventListItem
                {
                    Date = evt.Date,
                    EventId = evt.Id,
                    MaxParticipants = evt.MaxSeats,
                    IsPrivate = evt.IsPrivate,
                    Time = evt.Time,
                    Participants = parts,
                    Title = evt.Title,
                    Owner=evt.Owner.Pseudo,
                    OwnerId=evt.Owner.Id
                };
                
                
                list.Add(display);
            }

            return list.OrderByDescending(g=>g.Date).ThenByDescending(g=>g.Time).ToList();
        }

        public async Task CreateNewEvent(CreateEventViewModel @event)
        {
            var userName = UserHelpers.UserName();
            var usr = await _db.Users.FirstOrDefaultAsync(u => u.Email == userName);
            var evt = new Event
            {
                Address = @event.Address,
                Date = @event.Date,
                Title = @event.Title,
                Description = @event.Description,
                IsPrivate = @event.IsPrivate,
                MaxSeats = @event.MaxParticipants,
                MeetingDetails = @event.MeetingPoint,
                Time = @event.Time
            };
            if (usr != null) evt.Owner = usr;
            _db.Events.Add(evt);
            await AddParticipation(evt);
            await _db.SaveChangesAsync();
        }

        public async Task EditEvent(CreateEventViewModel @event)
        {
            var exists = await _db.Events.FirstOrDefaultAsync(r => r.Id == @event.Id);

            if (exists != null)
            {
                exists.Address = @event.Address;
                exists.Date = @event.Date;
                exists.Title = @event.Title;
                exists.Description = @event.Description;
                exists.IsPrivate = @event.IsPrivate;
                exists.MaxSeats = @event.MaxParticipants;
                exists.MeetingDetails = @event.MeetingPoint;
                exists.Time = @event.Time;
            }

            _db.Entry(exists).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task<EventDataDetailsViewModel> GetEventInfos(Event targetEvent)
        {

            var usr = await GetCurrentUser();
            var dataDuration = _db.UserToEventsData.Include(t => t.Event).Include(f => f.User).Where(evf => evf.User.Id == usr.Id && evf.Event.Id == targetEvent.Id).ToList();

            for (int i = 0; i < Enum.GetNames(typeof(DirectionModes)).Length; i++)
            {
                DirectionModes md = (DirectionModes)i;
                var direction = await _apiHelper.GetDirections(usr.Address, targetEvent.Address, "metric", md);
                var dataobje = ApiObjectToEventData(direction, usr, targetEvent, md);
                var exists = dataDuration.FirstOrDefault(d => d.DirectionMode == md);

                if (exists != null)
                {
                    exists.Distance = dataobje.Distance;
                    exists.Duration = dataobje.Duration;
                    exists.MapLink = dataobje.MapLink;
                    _db.Entry(exists).State = EntityState.Modified;
                }
                else
                {
                    _db.UserToEventsData.Add(dataobje);
                }
            }
            await _db.SaveChangesAsync();

            var parts = await _db.Participations.Include(p => p.Event).Include(p => p.User).Where(p => p.Event.Id == targetEvent.Id).ToListAsync();


            var isCurrentUserTheOwner = usr != null && (targetEvent.Owner != null && targetEvent.Owner.Id == usr.Id);

            return new EventDataDetailsViewModel { Event = targetEvent, UserData = dataDuration, CanModify = isCurrentUserTheOwner, Participants = parts };
        }



        private EventRelativeToUserInformation ApiObjectToEventData(DirectionObject dObj, ApplicationUser usr,
            Event targetEvent, DirectionModes md)
        {
            var data = new EventRelativeToUserInformation
            {
                User = usr,
                Event = targetEvent,
                DirectionMode = md,
                Unit = "km",
                Distance = (dObj.routes.Any() ? dObj.routes.FirstOrDefault().legs.FirstOrDefault().distance.value / 1000.0 : 0.0),
                Duration = (dObj.routes.Any() ? dObj.routes.FirstOrDefault().legs.FirstOrDefault().duration.value / 60 : 0),
                MapLink = _apiHelper.GenerateMapLink(usr.Address, targetEvent.Address, md)
            };
            return data;
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            var userName = UserHelpers.UserName();
            var usr = await _db.Users.FirstOrDefaultAsync(u => u.Email == userName);
            return usr;
        }
    }
}