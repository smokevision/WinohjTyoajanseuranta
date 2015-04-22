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
    public class BulletinController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bulletin/Create
        public ActionResult Create()
        {
            var selectItems = from item in db.Users
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = item.Id
                              };

            ViewBag.ApplicationUserID = selectItems;
            return View();
        }

        // POST: Bulletin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ApplicationUserID,Header,Content,Date")] Bulletin bulletin)
        {
            if (ModelState.IsValid)
            {
                bulletin.Date = DateTime.Now;
                db.Bulletins.Add(bulletin);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(bulletin);
        }

        // GET: Bulletin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bulletin bulletin = db.Bulletins.Find(id);
            if (bulletin == null)
            {
                return HttpNotFound();
            }

            var selectItems = from item in db.Users
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = bulletin.ApplicationUserID
                              };

            ViewBag.ApplicationUserID = selectItems;
            return View(bulletin);
        }

        // POST: Bulletin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ApplicationUserID,Header,Content,Date")] Bulletin bulletin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bulletin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            var selectItems = from item in db.Users
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = bulletin.ApplicationUserID
                              };
            ViewBag.ApplicationUserID = selectItems;
            return View(bulletin);
        }

        // GET: Bulletin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bulletin bulletin = db.Bulletins.Find(id);
            if (bulletin == null)
            {
                return HttpNotFound();
            }
            return View(bulletin);
        }

        // POST: Bulletin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bulletin bulletin = db.Bulletins.Find(id);
            db.Bulletins.Remove(bulletin);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
