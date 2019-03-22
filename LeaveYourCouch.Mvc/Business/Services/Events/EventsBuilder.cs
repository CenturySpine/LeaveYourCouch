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
            var dataDuration = _db.UserToEventsData.Include(t => t.Event).Include(f => f.User)
                   .FirstOrDefault(evf => evf.User.Id == usr.Id && evf.Event.Id == targetevent.Id);
            if (dataDuration == null)
            {
                if (usr != null)
                {
                    dataDuration = await BuildDurations(targetevent, usr);
                }
            }

            return new EventDataDetailsViewModel { Event = targetevent, UserData = dataDuration };
        }

        private async Task<EventRelativeToUserInformation> BuildDurations(Event targetevent, ApplicationUser usr)
        {
            EventRelativeToUserInformation dataDuration;
            var direction = await _apiHelper.GetDirections(usr.PostalCode, targetevent.Address, "metric");
            dataDuration = new EventRelativeToUserInformation
            {
                User = usr,
                Event = targetevent,
                Distance =
                    (direction.FirstOrDefault().Value.routes.FirstOrDefault().legs.FirstOrDefault().distance.value / 1000.0),
                DurationCycling =
                    (direction[DirectionModes.bicycling].routes.FirstOrDefault().legs.FirstOrDefault().duration.value / 60),
                DurationDriving =
                    (direction[DirectionModes.driving].routes.FirstOrDefault().legs.FirstOrDefault().duration.value / 60),
                DurationTransit =
                    (direction[DirectionModes.transit].routes.FirstOrDefault().legs.FirstOrDefault().duration.value / 60),
                DurationWalking =
                    (direction[DirectionModes.walking].routes.FirstOrDefault().legs.FirstOrDefault().duration.value / 60),
            };
            _db.UserToEventsData.Add(dataDuration);
            await _db.SaveChangesAsync();
            return dataDuration;
        }
    }
}