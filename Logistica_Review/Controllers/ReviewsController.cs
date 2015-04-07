using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid;//added to implement the grid

namespace Logistica_Review.Controllers
{
    [RequireHttps]
    public class ReviewsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Evaluation(string id)
        {
            //ViewData["id"] = id;

            return View();
        }
    }
}