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
    public class BulletinController : Controller
    {
        private AppContext db = new AppContext();

        // GET: Bulletin/Create
        public ActionResult Create()
        {
            var selectItems = from item in db.Employees
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = item.ID.ToString()
                              };

            ViewBag.EmployeeID = selectItems;
            return View();
        }

        // POST: Bulletin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeID,Header,Content,Date")] Bulletin bulletin)
        {
            if (ModelState.IsValid)
            {
                bulletin.Date = DateTime.Now;
                db.Bulletins.Add(bulletin);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.EmployeeID = new SelectList(db.Employees, "ID", "FirstName", bulletin.EmployeeID);
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

            var selectItems = from item in db.Employees
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = bulletin.EmployeeID.ToString()
                              };

            ViewBag.EmployeeID = selectItems;
            return View(bulletin);
        }

        // POST: Bulletin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeID,Header,Content,Date")] Bulletin bulletin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bulletin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            var selectItems = from item in db.Employees
                              select new SelectListItem
                              {
                                  Text = item.FirstName + " " + item.LastName,
                                  Value = bulletin.EmployeeID.ToString()
                              };

            ViewBag.EmployeeID = selectItems;
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
