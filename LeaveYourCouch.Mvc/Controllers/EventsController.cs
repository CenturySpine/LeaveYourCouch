using LeaveYourCouch.Mvc.Business.Services.Events;
using LeaveYourCouch.Mvc.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LeaveYourCouch.Mvc.Controllers
{
    public enum EventMessagesIds
    {
        SubscribtionDone,
        UnusbscriptionDone
    }
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEventsBuilder _eventBuilder;

        public EventsController(ApplicationDbContext db, IEventsBuilder eventBuilder)
        {
            _db = db;
            _eventBuilder = eventBuilder;
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View(new CreateEventViewModel());
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateEventViewModel @event)
        {
            if (ModelState.IsValid)
            {
                await _eventBuilder.CreateNewEvent(@event);

                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await _db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Event @event = await _db.Events.FindAsync(id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            await _eventBuilder.ConfirmEventDeletionAsync(@event);

            return RedirectToAction("Index");
        }

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id, EventMessagesIds? message)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await _db.Events.Include(e => e.Owner).FirstOrDefaultAsync(r => r.Id == id);
            if (@event == null)
            {
                return HttpNotFound();
            }


            var eventinfo = await _eventBuilder.GetEventInfos(@event);

            ViewBag.StatusMessage =
                message == EventMessagesIds.SubscribtionDone ? "Subscription successful"
                    : message == EventMessagesIds.UnusbscriptionDone ? "Unsubscription successful"

                : "";

            return View(eventinfo);
        }

        // GET: Events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await _db.Events.Include(e => e.Owner).FirstOrDefaultAsync(e => e.Id == id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(new CreateEventViewModel() { Id = @event.Id, Owner = @event.Owner, Date = @event.Date, Title = @event.Title, MeetingPoint = @event.MeetingDetails, MaxParticipants = @event.MaxSeats, Time = @event.Time, Address = @event.Address, IsPrivate = @event.IsPrivate, Description = @event.Description });
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateEventViewModel @event)
        {
            if (ModelState.IsValid)
            {
                await _eventBuilder.EditEvent(@event);

                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Events
        public async Task<ActionResult> Index()
        {
            var events = await _eventBuilder.ListEvents();
            return View(events);
        }

        // GET: Events/Subscribe/5
        public async Task<ActionResult> Subscribe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await _db.Events.FindAsync(id);

            if (@event == null)
            {
                return HttpNotFound();
            }

            await _eventBuilder.AddParticipation(@event);

            return RedirectToAction("Details", new { id, Message = EventMessagesIds.SubscribtionDone });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> UnSubscribe(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await _db.Events.FindAsync(id);
            await _eventBuilder.RemoveParticipation(@event);

            if (@event == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Details", new { id, Message = EventMessagesIds.UnusbscriptionDone });

        }
    }
}