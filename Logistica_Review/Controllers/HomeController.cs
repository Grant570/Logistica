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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult MyEvaluations()
        {
            DatabaseManager dbm = new DatabaseManager();
            List<ProjectModel> projects = dbm.getReviews(User.Identity.GetUserId());
            return View(projects);
        }
    }
}