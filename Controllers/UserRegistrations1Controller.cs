using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using LMS.Models;

namespace LMS.Controllers
{
    public class UserRegistrations1Controller : Controller
    {
        private DbModels db = new DbModels();

        // GET: UserRegistrations1
        public ActionResult Index()
        {
            return View(db.UserRegistrations.ToList());
        }

        // GET: UserRegistrations1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration userRegistration = db.UserRegistrations.Find(id);
            if (userRegistration == null)
            {
                return HttpNotFound();
            }
            return View(userRegistration);
        }

        // GET: UserRegistrations1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRegistrations1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,Email,MobileNo,Address,Password,UserType")] UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(userRegistration.UserType)) userRegistration.UserType = "1";
                db.UserRegistrations.Add(userRegistration);
                db.SaveChanges();
                if (userRegistration.UserName != null)
                {
                    try
                    {
                        MailMessage mm = new MailMessage("laundryservice.lms@gmail.com", userRegistration.Email);
                        mm.Subject = "Registration Details";
                        mm.Body = "Dear " + userRegistration.UserName + ", Your User Name is: " + userRegistration.UserName + " && Password is: " + userRegistration.Password + ".";
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential();
                        NetworkCred.UserName = "laundryservice.lms@gmail.com";
                        NetworkCred.Password = "Lms@642692";
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    return RedirectToAction("AdminDashboard", "Login");
                }
            }

            return View(userRegistration);
        }

        // GET: UserRegistrations1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration userRegistration = db.UserRegistrations.Find(id);
            if (userRegistration == null)
            {
                return HttpNotFound();
            }
            return View(userRegistration);
        }

        // POST: UserRegistrations1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Email,MobileNo,Address,Password,UserType")] UserRegistration userRegistration)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userRegistration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userRegistration);
        }

        // GET: UserRegistrations1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration userRegistration = db.UserRegistrations.Find(id);
            if (userRegistration == null)
            {
                return HttpNotFound();
            }
            return View(userRegistration);
        }

        // POST: UserRegistrations1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRegistration userRegistration = db.UserRegistrations.Find(id);
            db.UserRegistrations.Remove(userRegistration);
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
