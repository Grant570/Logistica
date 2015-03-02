using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logistica_Review.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewReviews()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Review()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}