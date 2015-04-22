using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TyoaikaApp.Models;
using Microsoft.AspNet.Identity;

namespace TyoaikaApp.Controllers
{
    public class TimesheetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Timesheet
        public ActionResult Index()
        {
            String userId = User.Identity.GetUserId();
            var timesheet = db.Timesheets.Include(t => t.ApplicationUser).Where(i => i.ApplicationUserID == userId && i.Date == DateTime.Today).SingleOrDefault();
            return View(timesheet);
        }

        // POST: Timesheet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "TimesheetID,ApplicationUserID,Date,Information,LunchBreak")] Timesheet timesheet, string submitButton)
        {
            String userId = User.Identity.GetUserId();
            
            if (ModelState.IsValid)
            {
                var currentTimesheet = db.Timesheets.Include(t => t.ApplicationUser).Where(i => i.ApplicationUserID == userId && i.Date == DateTime.Today).SingleOrDefault();
                if (submitButton == "Save")
                {
                    currentTimesheet.LunchBreak = timesheet.LunchBreak;
                    currentTimesheet.Information = timesheet.Information;
                    db.Entry(currentTimesheet).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (currentTimesheet == null)
                {
                    //no timesheet data was found, so start day
                    timesheet.Date = DateTime.Today;
                    timesheet.ApplicationUserID = User.Identity.GetUserId();
                    timesheet.Information = "";
                    timesheet.LunchBreak = true;
                    db.Timesheets.Add(timesheet);
                    db.SaveChanges();
                    TimesheetRow timesheetRow = new TimesheetRow();
                    timesheetRow.TimesheetID = timesheet.TimesheetID;
                    timesheetRow.StartTime = DateTime.Now;
                    db.TimesheetRows.Add(timesheetRow);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (currentTimesheet.TimesheetRows.Last().StopTime != null)
                {
                    //there is timesheet data for today and last timesheetrow is complete, start new
                    TimesheetRow timesheetRow = new TimesheetRow();
                    timesheetRow.TimesheetID = currentTimesheet.TimesheetID;
                    timesheetRow.StartTime = DateTime.Now;
                    currentTimesheet.TimesheetRows.Add(timesheetRow);
                    db.Entry(currentTimesheet).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else if (currentTimesheet.TimesheetRows.Last().StopTime == null)
                {
                    //end current timesheetrow
                    currentTimesheet.TimesheetRows.Last().StopTime = DateTime.Now;
                    db.Entry(currentTimesheet).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            var ts = db.Timesheets.Include(t => t.ApplicationUser).Where(i => i.ApplicationUserID == userId && i.Date == DateTime.Today).SingleOrDefault();
            return View(ts);
        }

        // GET: Timesheet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timesheet timesheet = db.Timesheets.Find(id);
            if (timesheet == null)
            {
                return HttpNotFound();
            }
            return View(timesheet);
        }

        // GET: Timesheet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Timesheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TimesheetID,ApplicationUserID,Date,Information,LunchBreak")] Timesheet timesheet)
        {
            if (ModelState.IsValid)
            {
                db.Timesheets.Add(timesheet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(timesheet);
        }

        // GET: Timesheet/Edit
        public ActionResult Edit()
        {
            return View();
        }

        // POST: Timesheet/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TimesheetID,ApplicationUserID,Date,Information,LunchBreak")] Timesheet timesheet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timesheet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timesheet);
        }

        // GET: Timesheet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timesheet timesheet = db.Timesheets.Find(id);
            if (timesheet == null)
            {
                return HttpNotFound();
            }
            return View(timesheet);
        }

        // POST: Timesheet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Timesheet timesheet = db.Timesheets.Find(id);
            db.Timesheets.Remove(timesheet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
