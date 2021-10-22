using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zmedic.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //------------------------------------- หน้าแรก -------------------------------------------------------
        public ActionResult Index()
        {
            return View();
        }
    }
}