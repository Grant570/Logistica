using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcContrib.UI.Grid;//added to implement the grid

namespace Logistica_Review.Controllers
{
    [RequireHttps]
    public class ProjectsController : Controller
    {
        public ActionResult AddUser()
        {
            if(Request.IsAjaxRequest()) {
                throw new Exception();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            return PartialView();
        }

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            ViewData["UserName"] = User.Identity.Name;
            return View();
        }

        public ActionResult Evaluations(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            ViewData["id"] = id;
            return View();
        }

        public ActionResult CreateProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            return View();
        }

        public ActionResult EditProject()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            return View();
        }
    }
}