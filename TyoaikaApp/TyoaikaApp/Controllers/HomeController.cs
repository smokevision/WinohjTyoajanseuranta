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
    [Authorize]
    public class HomeController : Controller
    {
        private AppContext db = new AppContext();
        public ActionResult Index()
        {
            var bulletins = db.Bulletins.Include(b => b.Employee);
            return View(bulletins.ToList());
        }
    }
}