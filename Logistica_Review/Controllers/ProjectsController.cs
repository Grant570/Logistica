using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
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

            List<ProjectModel> projects = dbm.getManagingProjects(User.Identity.GetUserId());
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

        [HttpPost]
        public ActionResult AddProject()
        {
            string name = Request.Form["title"];
            string description = Request.Form["description"];
            string dueDate = Request.Form["due-date"];
            List<string> users = new List<string>();
            List<string> questions = new List<string>();

            int i = 0;
            while(true) {
                if(Request.Form["user-" + i] != null) {
                    users.Add(Request.Form["user-" + i]);
                }
                else
                {
                    break;
                }
                i++;
            }

            i = 0;
            while (true)
            {
                if (Request.Form["question-" + i] != null)
                {
                    questions.Add(Request.Form["question-" + i]);
                }
                else
                {
                    break;
                }
                i++;
            }

            DatabaseManager dbm = new DatabaseManager();
            dbm.addProject(User.Identity.GetUserId(), name, description, dueDate, users, questions);

            return RedirectToAction("Index");
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