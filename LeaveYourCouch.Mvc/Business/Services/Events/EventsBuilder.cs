using System.Collections.Generic;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Models;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveYourCouch.Mvc.Business.Services.Events
{
    public interface IEventsBuilder
    {
        Task<EventDataDetailsViewModel> GetUserEventInfos(Event targetevent);
        Task CreateNewEvent(CreateEventViewModel @event);
    }

    public class EventsBuilder : IEventsBuilder
    {
        private readonly IApiHelper _apiHelper;
        private readonly ApplicationDbContext _db;

        public EventsBuilder(ApplicationDbContext db, IApiHelper apiHelper)
        {
            _db = db;
            _apiHelper = apiHelper;
        }

        public async Task<EventDataDetailsViewModel> GetUserEventInfos(Event targetevent)
        {
            EventDataDetailsViewModel vm = new EventDataDetailsViewModel();
            var usrmail = UserHelpers.UserName();
            var usr = await _db.Users.FirstOrDefaultAsync(u => u.Email == usrmail);
            var dataDuration = _db.UserToEventsData.Include(t => t.Event).Include(f => f.User).Where(evf => evf.User.Id == usr.Id && evf.Event.Id == targetevent.Id).ToList();


            if (dataDuration.Count <= 0)
            {
                if (usr != null)
                {
                    dataDuration = await BuildDurations(targetevent, usr);
                }
            }

            var iscurrentuserTheOwner = usr != null && (targetevent.Owner != null && targetevent.Owner.Id == usr.Id);
            return new EventDataDetailsViewModel { Event = targetevent, UserData = dataDuration,CanModify= iscurrentuserTheOwner };
        }

        public async Task CreateNewEvent(CreateEventViewModel @event)
        {
            var usrmail = UserHelpers.UserName();
            var usr = await _db.Users.FirstOrDefaultAsync(u => u.Email == usrmail);
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
            await _db.SaveChangesAsync();
        }

        private async Task<List<EventRelativeToUserInformation>> BuildDurations(Event targetevent, ApplicationUser usr)
        {
            List<EventRelativeToUserInformation> dataDuration = new List<EventRelativeToUserInformation>();
            var direction = await _apiHelper.GetDirections(usr.Address, targetevent.Address, "metric");

            foreach (var dobj in direction)
            {
                var data = new EventRelativeToUserInformation
                {
                    User = usr,
                    Event = targetevent,
                    DirectionMode = dobj.Key,
                    Unit = "km",
                    Distance = (dobj.Value.routes.Any() ? dobj.Value.routes.FirstOrDefault().legs.FirstOrDefault().distance.value / 1000.0 : 0.0),
                    Duration = (dobj.Value.routes.Any() ? dobj.Value.routes.FirstOrDefault().legs.FirstOrDefault().duration.value / 60:0),
                    MapLink= _apiHelper.GenerateMapLink(usr.Address, targetevent.Address,dobj.Key)
                };
                dataDuration.Add(data);
                _db.UserToEventsData.Add(data);

            }

            await _db.SaveChangesAsync();
            return dataDuration;
        }
    }
}