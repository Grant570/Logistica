using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logistica_Review.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ViewReviews()
        {
            ViewBag.Message = "View your reviews here.";

            return View();
        }

        public ActionResult Review()
        {
            ViewBag.Message = "Review your colleagues.";

            return View();
        }
    }
}