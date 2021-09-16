using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Zmedic.Models;
using System.Data.Entity;

namespace Zmedic.Controllers
{
    public class LoginController : Controller
    {
        AccZmedicEntities _context = new AccZmedicEntities();

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var passwordGetMD5 = GetMD5(password);
                var member = _context.Admin.Where(p => p.User.Equals(username) && p.Password.Equals(passwordGetMD5)).ToList();

                if (member.Count() > 0)
                {
                    Session["Username"] = member.FirstOrDefault().User.ToString();
                    Session["Role"] = member.FirstOrDefault().Role.ToString();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult ChangePassword(int id)
        {
            Admin admin = _context.Admin.Find(id);

            if (admin == null)
            {
                RedirectToAction("Index", "Login");
            }

            return View(admin);

        }


        [HttpPost]
        public ActionResult ChangePassword(Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.Password = GetMD5(admin.Password);
                _context.Admin.Attach(admin);
                _context.Entry(admin).Property(a => a.Password).IsModified = true;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Password Hash MD5
        public string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(password);
            byte[] targetData = md5.ComputeHash(fromData);

            string byteToString = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byteToString += targetData[i].ToString("osde17613");
            }
            return byteToString;

        }
    }
}