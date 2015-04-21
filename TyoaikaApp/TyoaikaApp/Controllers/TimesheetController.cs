using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TyoaikaApp.DAL;
using TyoaikaApp.Models;

namespace TyoaikaApp.Controllers
{
    public class TimesheetController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Timesheet
        public ActionResult Index()
        {
            var timesheet = db.Timesheets.Include(t => t.Employee).Where(i => i.EmployeeID == 1 && i.Date == DateTime.Today).SingleOrDefault();
            return View(timesheet);
        }

        // POST: Timesheet
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "TimesheetID,EmployeeID,Date,Information,LunchBreak")] Timesheet timesheet)
        {
            if (ModelState.IsValid)
            {
                var currentTimesheet = db.Timesheets.Include(t => t.Employee).Where(i => i.EmployeeID == 1 && i.Date == DateTime.Today).SingleOrDefault();

                if (currentTimesheet == null)
                {
                    timesheet.Date = DateTime.Today;
                    timesheet.EmployeeID = 1;
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
                    currentTimesheet.TimesheetRows.Last().StopTime = DateTime.Now;
                    db.Entry(currentTimesheet).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            var ts = db.Timesheets.Include(t => t.Employee).Where(i => i.EmployeeID == 1 && i.Date == DateTime.Today).SingleOrDefault();
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
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName");
            return View();
        }

        // POST: Timesheet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TimesheetID,EmployeeID,Date,Information,LunchBreak")] Timesheet timesheet)
        {
            if (ModelState.IsValid)
            {
                db.Timesheets.Add(timesheet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", timesheet.EmployeeID);
            return View(timesheet);
        }

        // GET: Timesheet/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", timesheet.EmployeeID);
            return View(timesheet);
        }

        // POST: Timesheet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TimesheetID,EmployeeID,Date,Information,LunchBreak")] Timesheet timesheet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timesheet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", timesheet.EmployeeID);
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
