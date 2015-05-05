using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TyoaikaApp.Models;

namespace TyoaikaApp.Controllers
{
    [Authorize(Roles = "ROLE_ADMIN, ROLE_SUPER_ADMIN")]
    public class ReportController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Index()
        {
            var selectItems = from item in db.Users 
                              where item.UserName != "admin"
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = item.Id
                              };
            ViewBag.ApplicationUserID = selectItems;
            return View();
        }

        // POST: Report
        [HttpPost]
        public ActionResult Index(Report report)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Compose", report);
            }


            return View(report);
        }

        // GET: Report/Compose
        public ActionResult Compose(Report report)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = db.Users.Find(report.ApplicationUserID);
                report.ApplicationUser = user;
                var timesheets = db.Timesheets.Where( t => t.ApplicationUserID == report.ApplicationUserID && t.Date >= report.StartDate && t.Date <= report.EndDate);
                List<Timesheet> timesheetList = timesheets.ToList();
                report.ReportRows = new List<ReportRow>();
                //total worktimes
                report.TimeSum = TimeSpan.Zero;
                report.TimeTotal = TimeSpan.Zero;
                report.LunchBreaks = 0;
                report.LunchBreakTime = TimeSpan.Zero;

                for (var dt = report.StartDate; dt <= report.EndDate; dt = dt.AddDays(1))
                {
                    ReportRow reportRow = new ReportRow();
                    reportRow.Date = dt;
                    if (timesheetList.FindIndex(t => t.Date == dt) >= 0)
                    {
                        //there is timesheet data for the day in question
                        reportRow.Timesheet = timesheetList.Find(t => t.Date == dt);
                        //count total worktime for this day
                        foreach (var timesheetRow in reportRow.Timesheet.TimesheetRows)
                        {
                            if (timesheetRow.StopTime != null)
                            {
                                report.TimeSum = report.TimeSum.Add(timesheetRow.StopTime.Value - timesheetRow.StartTime);
                            }
                        }

                        if (reportRow.Timesheet.LunchBreak)
                        {
                            report.LunchBreaks++;
                            report.LunchBreakTime = report.LunchBreakTime.Add(TimeSpan.FromMinutes(30));
                        }

                    }
                    report.ReportRows.Add(reportRow);
                }

                report.TimeTotal = report.TimeSum;
                report.TimeTotal = report.TimeTotal.Subtract(TimeSpan.FromMinutes(report.LunchBreaks * 30));

                return View(report);
            }
            return RedirectToAction("Index");
        }

        
    }
}
