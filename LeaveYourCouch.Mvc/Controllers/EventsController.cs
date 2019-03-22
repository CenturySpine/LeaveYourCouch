using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LeaveYourCouch.Mvc.Business;
using LeaveYourCouch.Mvc.Business.Services.Events;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IApiHelper _apiHelper;
        private readonly IEventsBuilder _eventBuilder;

        public EventsController(ApplicationDbContext db, IApiHelper apiHelper, IEventsBuilder eventBuilder)
        {
            _db = db;
            _apiHelper = apiHelper;
            _eventBuilder = eventBuilder;
        }

        // GET: Events
        public async Task<ActionResult> Index()
        {
            return View(await _db.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<ActionResult> Details(int? id)
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

            //the user did not display the event yet, so we have to retreive the personnal informations regarding distance and duration
            //and save it to avoid requesting data on next visit
            var dataDuration = await _eventBuilder.GetUserEventInfos(@event);





            return View(dataDuration);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            return View(new CreateEventViewModel());
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateEventViewModel @event)
        {
            if (ModelState.IsValid)
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
            _db.Events.Remove(@event);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
