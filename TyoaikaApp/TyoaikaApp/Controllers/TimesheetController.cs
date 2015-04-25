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
using System.Globalization;

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

        // GET: Timesheet/Manage
        public ActionResult Manage()
        {

            var selectItems = from item in db.Users
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = item.Id
                              };

            ViewBag.ApplicationUsers = selectItems;

            return View();
        }

        // POST: Timesheet/Manage
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(Timesheet timesheet, string ApplicationUsers, string inputDate, string submitButton)
        {
            var selectItems = from item in db.Users
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = item.Id
                              };

            ViewBag.ApplicationUsers = selectItems;

            if (submitButton == "search")
            {
                //user has searched for a day
                string DATE_FORMAT = "dd.MM.yyyy";
                DateTime searchDate;
                if (DateTime.TryParseExact(inputDate, DATE_FORMAT, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out searchDate))
                {
                    Timesheet curTimesheet = db.Timesheets.Where(t => t.ApplicationUserID == ApplicationUsers && t.Date == searchDate).SingleOrDefault();
                    if (curTimesheet == null)
                    {
                        ApplicationUser timesheetUser = db.Users.Where(u => u.Id == ApplicationUsers).Single();
                        timesheet.ApplicationUserID = ApplicationUsers;
                        timesheet.ApplicationUser = timesheetUser;
                        timesheet.Date = searchDate;
                        timesheet.Information = "";
                        timesheet.LunchBreak = true;
                    }
                    else
                    {
                        timesheet = curTimesheet;
                    }
                    return View(timesheet);
                }else
                {
                    ViewBag.messageError = "Virheellinen päivämäärä.";
                    return View();
                }

            }
            else if (submitButton == "save")
            {
                //form with edited timesheet data was submitted
                string[] startTimes = Request.Form.GetValues("startTime");
                string[] stopTimes = Request.Form.GetValues("stopTime");
                Timesheet curTimesheet = db.Timesheets.Where(t => t.TimesheetID == timesheet.TimesheetID).SingleOrDefault();
                if (curTimesheet != null)
                {
                    //we have timesheet, update data
                    curTimesheet.Information = timesheet.Information;
                    curTimesheet.LunchBreak = timesheet.LunchBreak;
                    string date = curTimesheet.Date.ToString("dd.MM.yyyy");
                    //remove all old timesheetrows
                    db.TimesheetRows.RemoveRange(curTimesheet.TimesheetRows);
                    for (int i = 0; i < startTimes.Count(); i++)
                    {
                        TimesheetRow newRow = new TimesheetRow();
                        newRow.TimesheetID = curTimesheet.TimesheetID;
                        DateTime parsedStart;
                        DateTime parsedStop;
                        string startTime = curTimesheet.Date.ToString("dd.MM.yyyy") + " " + startTimes[i];
                        string stopTime = curTimesheet.Date.ToString("dd.MM.yyyy") + " " + stopTimes[i];
                        if (DateTime.TryParse(startTime, out parsedStart))
                        {
                            newRow.StartTime = parsedStart;
                        }
                        else
                        {
                            ViewBag.messageError = "Lomake sisälsi virheellisen alkuajan.";
                            return View(curTimesheet);
                        }

                        if (DateTime.TryParse(stopTime, out parsedStop))
                        {
                            newRow.StopTime = parsedStop;
                        }
                        else
                        {
                            newRow.StopTime = null;
                        }
                        db.TimesheetRows.Add(newRow);
                    }
                    db.Entry(curTimesheet).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.messageSuccess = "Päivän tiedot tallennettu.";
                    return View();
                }
                else
                {
                    //no timesheet found, create new
                    timesheet.ApplicationUserID = ApplicationUsers;
                    ApplicationUser user = db.Users.Find(ApplicationUsers);
                    timesheet.ApplicationUser = user;
                    timesheet.Information = timesheet.Information;
                    timesheet.LunchBreak = timesheet.LunchBreak;
                    db.Timesheets.Add(timesheet);

                    for (int i = 0; i < startTimes.Count(); i++)
                    {
                        TimesheetRow newRow = new TimesheetRow();
                        newRow.TimesheetID = timesheet.TimesheetID;
                        DateTime parsedStart;
                        DateTime parsedStop;
                        string startTime = timesheet.Date.ToString("dd.MM.yyyy") + " " + startTimes[i];
                        string stopTime = timesheet.Date.ToString("dd.MM.yyyy") + " " + stopTimes[i];
                        if (DateTime.TryParse(startTime, out parsedStart))
                        {
                            newRow.StartTime = parsedStart;
                        }
                        else
                        {
                            ViewBag.messageError = "Lomake sisälsi virheellisen alkuajan.";
                            return View(timesheet);
                        }

                        if (DateTime.TryParse(stopTime, out parsedStop))
                        {
                            newRow.StopTime = parsedStop;
                        }
                        else
                        {
                            newRow.StopTime = null;
                        }
                        db.TimesheetRows.Add(newRow);
                    }
                    db.SaveChanges();
                    ViewBag.messageSuccess = "Päivän tiedot tallennettu.";
                    return View();
                }
            }
            else
            {
                //something has gone wrong, just display the page
                return View();
            }
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
