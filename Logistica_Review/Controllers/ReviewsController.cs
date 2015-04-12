using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
            List<ProjectModel> projects = dbm.getAssignedProjects(User.Identity.GetUserId());
            ViewBag.UserId = User.Identity.GetUserId();
            return View(projects);
        }

        public ActionResult Evaluation(int projectId, string forUser, string submittedBy)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("Index", "Home");
            }
            DatabaseManager dbm = new DatabaseManager();
            EvaluationModel evaluation = dbm.getEvaluation(projectId, forUser, submittedBy);
            return View(evaluation);
        }
    }
}