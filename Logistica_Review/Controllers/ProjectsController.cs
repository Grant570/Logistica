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
            return PartialView();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Evaluations(string id)
        {
            ViewData["id"] = id;
            return View();
        }

        public ActionResult CreateProject()
        {
            return View();
        }

        public ActionResult EditProject()
        {
            return View();
        }
    }
}