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
    public class Events1Controller : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IApiHelper _apiHelper;
        private readonly IEventsBuilder _eventBuilder;

        public Events1Controller(ApplicationDbContext db, IApiHelper apiHelper, IEventsBuilder eventBuilder)
        {
            _db = db;
            _apiHelper = apiHelper;
            _eventBuilder = eventBuilder;
        }

        // GET: Events1
        public async Task<ActionResult> Index()
        {
            return View(await _db.Events.ToListAsync());
        }

        // GET: Events1/Details/5
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

        // GET: Events1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Events1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,MaxSeats,Address,MeetingDetails,IsPrivate,Date,Time")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _db.Events.Add(@event);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Events1/Edit/5
        public async Task<ActionResult> Edit(int? id)
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

        // POST: Events1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,MaxSeats,Address,MeetingDetails,IsPrivate,Date,Time")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(@event).State = EntityState.Modified;
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
