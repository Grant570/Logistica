using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Logistica_Review.Database;
using Logistica_Review.Models;
using MvcContrib.UI.Grid;//added to implement the grid

namespace Logistica_Review.Controllers
{
    [RequireHttps]
    public class ReviewsController : Controller
    {
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) {
                return View("Index", "Home");
            }
            DatabaseManager dbm = new DatabaseManager();
            List<ProjectModel> projects = dbm.getAssignedProjects("a081ee30-5fa5-47f4-9312-3862bc2ce4a8");
            return View(projects);
        }

        public ActionResult Evaluation(string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            ViewData["id"] = id;
            return View();
        }
    }
}