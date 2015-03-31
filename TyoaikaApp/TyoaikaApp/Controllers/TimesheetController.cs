using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TyoaikaApp.Controllers
{
    [Authorize]
    public class TimesheetController : Controller
    {
        // GET: Timesheet
        public ActionResult Index()
        {
            return View();
        }
    }
}