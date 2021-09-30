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
    [HandleError]
    public class LoginController : Controller
    {
        AccZmedicEntities _context = new AccZmedicEntities();

        // GET: Login
        [Route("Ubx3BD")]
        public ActionResult Index()
        {
            return View();
        }


        [Route("Ubx3BD")]
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var passwordGetMD5 = GetMD5(password);
                var member = _context.Admin.Where(p => p.User.Equals(username) && p.Password.Equals(passwordGetMD5)).ToList();

                if (member.Count() > 0)
                {
                    Session["Id"] = member.FirstOrDefault().Id.ToString();
                    Session["Username"] = member.FirstOrDefault().User.ToString();
                    Session["Role"] = member.FirstOrDefault().Role.ToString();

                    return RedirectToAction("Index", "AdminPanel");
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
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                Admin admin = _context.Admin.Find(id);

                if (admin == null)
                {
                    RedirectToAction("Index", "Login");
                }

                return View(admin);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(Admin admin)
        {
            var currentPasswordFind = _context.Admin.Where(p => p.Password == admin.Password).FirstOrDefault();

            if (currentPasswordFind != null)
            {
                var user = _context.Admin.FirstOrDefault(p => p.Password == admin.Password);

                if (ModelState.IsValid)
                {
                    admin.Password = GetMD5(admin.Password);
                    user.Password = admin.Password;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("ChangePassword");
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