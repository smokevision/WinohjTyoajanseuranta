using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TyoaikaApp.Models;
namespace TyoaikaApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var bulletins = db.Bulletins.Include(b => b.ApplicationUser);
            return View(bulletins.ToList());
        }
    }
}