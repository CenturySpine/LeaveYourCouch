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

            return new EventDataDetailsViewModel { Event = targetevent, UserData = dataDuration };
        }

        private async Task<List<EventRelativeToUserInformation>> BuildDurations(Event targetevent, ApplicationUser usr)
        {
            List<EventRelativeToUserInformation> dataDuration = new List<EventRelativeToUserInformation>();
            var direction = await _apiHelper.GetDirections(usr.PostalCode, targetevent.Address, "metric");

            foreach (var dobj in direction)
            {
                var data = new EventRelativeToUserInformation
                {
                    User = usr,
                    Event = targetevent,
                    DirectionMode = dobj.Key,
                    Unit = "km",
                    Distance = (dobj.Value.routes.FirstOrDefault().legs.FirstOrDefault().distance.value) / 1000.0,
                    Duration = (dobj.Value.routes.FirstOrDefault().legs.FirstOrDefault().duration.value) / 60,
                };
                dataDuration.Add(data);
                _db.UserToEventsData.Add(data);

            }

            await _db.SaveChangesAsync();
            return dataDuration;
        }
    }
}