using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zmedic.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult PatientLabSearch()
        {
            return View();
        }

        public ActionResult LabResultDate()
        {
            return View();
        }

        public ActionResult Result()
        {
            return View();
        }
    }
}