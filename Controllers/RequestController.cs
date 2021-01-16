using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class RequestController : Controller
    {
        private DbModels db = new DbModels();
        // GET: Request
        public ActionResult Index()
        {
            var v = Session["UserName"].ToString();
            UserRegistration user = db.UserRegistrations.Where(i => i.UserName == v).FirstOrDefault();
            ViewBag.Address = user.Address;
            ViewBag.MobileNo = user.MobileNo;
            ViewBag.Email = user.Email;
            var items = db.Items.ToList();
            if (items!=null)
            {
                ViewBag.data = items;
            }
            var services = db.Services.ToList();
            if (services != null)
            {
                ViewBag.info = services;
            }
            List<CustomerRequest> objRequest = new List<CustomerRequest>();
            return View(objRequest);
        }
        public JsonResult InsertCustomers(List<CustomerRequest> customers)
        {
            using (DbModels db = new DbModels())
            {
                var v = Session["UserName"].ToString();
                UserRegistration user = db.UserRegistrations.Where(i => i.UserName == v).FirstOrDefault();
               
                //Check for NULL.
                if (customers == null)
                {
                    customers = new List<CustomerRequest>();
                }
                 
                   
                //Loop and insert records.
                foreach (CustomerRequest customer in customers)
                {
                    if (customer.RequestId == null)
                    {
                        var m = db.CustomerRequests.Count();
                        if(m == 0)
                        {
                            customer.RequestId = 1;
                        }
                        else
                        {
                            var n = db.CustomerRequests.Max(item => item.RequestId);
                            customer.RequestId = n + 1;
                        }
                        if (customer.RequestDate == null)
                        {
                            customer.RequestDate = DateTime.Now;
                        }
                        if (customer.UserId == null)
                        {
                            customer.UserId = user.UserId;
                            db.CustomerRequests.Add(customer);
                        }
                    }
                }
                int insertedRecords = db.SaveChanges();
                return Json(insertedRecords);
            }
        }
        public ActionResult RequestList()
        {
            return View(db.CustomerRequests.ToList());
        }

        public ActionResult Details(int RequestId)
        {
            List<CustomerRequest> list = new List<CustomerRequest>();
            var c = db.CustomerRequests.Where(x => x.RequestId == RequestId).ToList();
            var p = db.CustomerRequests.Where(x => x.RequestId == RequestId).Select(a => a.UserId).FirstOrDefault();
            var some = db.UserRegistrations.Where(b => b.UserId == p).FirstOrDefault();
            Session["Email"] = some.Email;
            Session["Name"] = some.UserName;
            Session["Address"] = some.Address;
            foreach (var i in list)
            {
               var s = c;
            }
            return View(c);
        }
        public ActionResult Total(int RequestId)
        {
            List<CustomerRequest> list = new List<CustomerRequest>();
            list = db.CustomerRequests.Where(x => x.RequestId == RequestId).ToList();
            List<Transaction> total = new List<Transaction>();
            Transaction t = new Transaction();
            if (list != null)
            { 
                foreach (var i in list)
            {
                        t.RequestId = i.RequestId;
                        t.UserId = i.UserId;
                        t.ItemId = i.ItemId;
                        t.ItemPrice = i.Item.Price;
                        t.ServiceId = i.ServiceId;
                        t.ServicePrice = i.Service.ServicePrice;
                        t.Quantity = i.Quantity;
                        t.Amount = (t.ItemPrice * t.Quantity) + t.ServicePrice;
                        t.Total = Convert.ToDecimal(t.Total + t.Amount);
                        db.Transactions.Add(t);
                        db.SaveChanges();
            }
                Session["Total"] = t.Total;
                Session["Id"] = t.RequestId;
                foreach (var i in list)
                {
                    if(i.RequestId != null)
                    {
                        i.Status = "Accepted";
                        db.SaveChanges();
                    }
                }
                    return RedirectToAction("Create", "Assignments");
                }
            
            return View(total);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMail(int RequestId, string txtReason)
        {
            List<CustomerRequest> list = new List<CustomerRequest>();
            list = db.CustomerRequests.Where(x => x.RequestId == RequestId).ToList();
            if (list != null)
            {
                foreach (var i in list)
                {
                    if (i.RequestId != null)
                    {
                        i.Status = "Rejected";
                        db.SaveChanges();
                    }
                }
                var email = Session["Email"].ToString();
                var name = Session["Name"].ToString();
                try
                {
                    MailMessage mm = new MailMessage("laundryservice.lms@gmail.com", email);
                    mm.Subject = "Laundry Service";
                    mm.Body = "Dear " + name + ", Your request rejected due to " + txtReason + ".";
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
                return RedirectToAction("RequestList");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptMail()
        {
            var email = Session["Email"].ToString();
            var name = Session["Name"].ToString();
            var date = Session["Date"].ToString();
            var time = Session["Time"].ToString();
            var pid = Convert.ToInt32(Session["Pid"].ToString());
            var pname = db.PickupBoys.Where(a => a.Pid == pid).Select(b => b.Name).FirstOrDefault();
            try
                    {
                        MailMessage mm = new MailMessage("laundryservice.lms@gmail.com", email);
                        mm.Subject = "Laundry Service";
                        mm.Body = "Dear " + name + ", Your request accepted." +
                            ""+pname+" will pickup your laundry items on "+ date +" at"+ time +".";
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
            return RedirectToAction("RequestList");
        }
        public ActionResult Status()
        {
            var v = Session["UserName"].ToString();
            var id = db.UserRegistrations.Where(a => a.UserName == v).Select(b => b.UserId).FirstOrDefault();
            return View(db.CustomerRequests.Where(a=>a.UserId == id ).ToList());
        }
        public ActionResult Bill(int RequestId)
        {
            return View(db.Transactions.Where(a => a.RequestId == RequestId).ToList());
        }
    }
}