using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    public class PickupBoysController : Controller
    {
        private DbModels db = new DbModels();

        // GET: PickupBoys
        public ActionResult Index()
        {
            return View(db.PickupBoys.ToList());
        }

        // GET: PickupBoys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickupBoy pickupBoy = db.PickupBoys.Find(id);
            if (pickupBoy == null)
            {
                return HttpNotFound();
            }
            return View(pickupBoy);
        }

        // GET: PickupBoys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PickupBoys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pid,Name,MobileNo,Email,Address")] PickupBoy pickupBoy)
        {
            if (ModelState.IsValid)
            {
                db.PickupBoys.Add(pickupBoy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pickupBoy);
        }

        // GET: PickupBoys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickupBoy pickupBoy = db.PickupBoys.Find(id);
            if (pickupBoy == null)
            {
                return HttpNotFound();
            }
            return View(pickupBoy);
        }

        // POST: PickupBoys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pid,Name,MobileNo,Email,Address")] PickupBoy pickupBoy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pickupBoy).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pickupBoy);
        }

        // GET: PickupBoys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickupBoy pickupBoy = db.PickupBoys.Find(id);
            if (pickupBoy == null)
            {
                return HttpNotFound();
            }
            return View(pickupBoy);
        }

        // POST: PickupBoys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PickupBoy pickupBoy = db.PickupBoys.Find(id);
            db.PickupBoys.Remove(pickupBoy);
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
