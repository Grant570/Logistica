using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Logistica_Review.Database;
using Logistica_Review.Models;
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

            DatabaseManager dbm = new DatabaseManager();
            List<ProjectModel> projects = dbm.getManagingProjects("a081ee30-5fa5-47f4-9312-3862bc2ce4a8");
            ViewData["UserName"] = User.Identity.Name;
            return View(projects);
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