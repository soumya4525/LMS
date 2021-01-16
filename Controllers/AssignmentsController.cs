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
    public class AssignmentsController : Controller
    {
        private DbModels db = new DbModels();

        // GET: Assignments
        public ActionResult Index()
        {
            var assignments = db.Assignments.Include(a => a.PickupBoy);
            return View(assignments.ToList());
        }

        // GET: Assignments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // GET: Assignments/Create
        public ActionResult Create()
        {
            ViewBag.Pid = new SelectList(db.PickupBoys, "Pid", "Name");
            return View();
        }

        // POST: Assignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssignId,AssignDate,AssignTime,Pid")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                Session["Date"] = assignment.AssignDate;
                Session["Time"] = assignment.AssignTime;
                Session["Pid"] = assignment.Pid;
                db.Assignments.Add(assignment);
                db.SaveChanges();
                if (assignment.Pid != null)
                {
                    var email = Session["Email"].ToString();
                    var name = Session["Name"].ToString();
                    var pemail = db.PickupBoys.Where(a => a.Pid == assignment.Pid).Select(b => b.Email).FirstOrDefault();
                    var pname = db.PickupBoys.Where(a => a.Pid == assignment.Pid).Select(b => b.Name).FirstOrDefault();
                    var address = Session["Address"].ToString();

                    try
                    {
                        MailMessage mm = new MailMessage("laundryservice.lms@gmail.com", pemail);
                        mm.Subject = "Laundry Service";
                        mm.Body = "Dear " + pname + ", " +
                            "You have to pickup laundry items on " + assignment.AssignDate + " at " + assignment.AssignTime + "." +
                            "Your pickup address is: " + address + ".";
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
                    
                    return RedirectToAction("Index","Pdf");
                }
            }
            Session["pname"] = assignment.PickupBoy.Name;
            ViewBag.Pid = new SelectList(db.PickupBoys, "Pid", "Name", assignment.Pid);
            return View(assignment);
        }

        // GET: Assignments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Pid = new SelectList(db.PickupBoys, "Pid", "Name", assignment.Pid);
            return View(assignment);
        }

        // POST: Assignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssignId,AssignDate,AssignTime,Pid")] Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assignment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Pid = new SelectList(db.PickupBoys, "Pid", "Name", assignment.Pid);
            return View(assignment);
        }

        // GET: Assignments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assignment assignment = db.Assignments.Find(id);
            if (assignment == null)
            {
                return HttpNotFound();
            }
            return View(assignment);
        }

        // POST: Assignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
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
