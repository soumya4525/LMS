using LMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class LoginController : Controller
    {
        public DbModels db = new DbModels();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserRegistration userModel)
        {

            var userDetails = db.UserRegistrations.Where(a => a.UserName == userModel.UserName
            && a.Password == userModel.Password).Select(b => b.UserType).FirstOrDefault();
            if (userDetails == null)
            {
                ViewBag.LoginErrorMessage = "Wrong UserName or Password";
                return View("Index", userModel);
            }
            else
            {
                if (userDetails == "0")
                {
                    Session["UserName"] = userModel.UserName;
                    return RedirectToAction("AdminDashboard", "Login");
                }
                else if (userDetails == "1")
                {
                    Session["UserName"] = userModel.UserName;
                    return RedirectToAction("ManagerDashboard", "Login");
                }
                else
                {
                    Session["UserName"] = userModel.UserName;
                    return RedirectToAction("UserDashboard", "Login");
                }
            }
        }
        public ActionResult AdminDashboard()
        {
            return View();
        }
        public ActionResult ManagerDashboard()
        {
            return View();
        }
        public ActionResult UserDashboard()
        {
            return View();
        }
        public ActionResult Profile()
        {
            var v = Session["UserName"].ToString();
            UserRegistration user = db.UserRegistrations.Where(i => i.UserName == v).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRegistration user = db.UserRegistrations.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,Email,MobileNo,Password,UserType")] UserRegistration user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(user);
        }
        public ActionResult ChangePassword(UserRegistration login)

        {
            using (DbModels db = new DbModels())
            {

                var detail = db.UserRegistrations.Where(log => log.Password == login.Password).FirstOrDefault();
                if (detail != null)
                {
                    var v = Session["UserName"].ToString();
                    var userDetail = db.UserRegistrations.FirstOrDefault(c => c.UserName == v);


                    if (userDetail != null)
                    {
                        userDetail.Password = login.NewPassword;

                        db.SaveChanges();
                        ViewBag.Message = "Record Inserted Successfully!";
                    }
                    else
                    {
                        ViewBag.Message = "Password not Updated!";
                    }

                }
            }
            return View();
        }

        public JsonResult CheckPass(string Password)
        {
            bool isValid = db.UserRegistrations.ToList().Exists(a => a.Password.Equals(Password, StringComparison.CurrentCultureIgnoreCase));
            return Json(isValid);
        }
    }
}
