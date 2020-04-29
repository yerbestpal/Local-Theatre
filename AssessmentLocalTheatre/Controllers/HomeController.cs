using AssessmentLocalTheatre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AssessmentLocalTheatre.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Instanciates the database context so the controllers may get context from it.
        /// </summary>
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        /// <summary>
        /// Pass the Posts to the View.
        /// </summary>
        /// <returns>View, including List of Posts.</returns>
        public ActionResult Index()
        {
            // Get Posts.
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff).Where(p => p.IsApproved).OrderByDescending(p => p.PostDate);

            // Get Categories and store in ViewBag.
            ViewBag.Categories = context.Categories.ToList();

            // Pass Posts to the view.
            return View(posts.ToList());
        }

        /// <summary>
        /// Search functionality that finds posts by title.
        /// </summary>
        /// <param name="SearchString">String passed into the action result and searchbox to search for posts.</param>
        /// <returns>View, including a list of found Posts.</returns>
        [HttpPost]
        public ViewResult Index(string SearchString)
        {
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff).Where(p => p.Title.Equals(SearchString.Trim())).OrderByDescending(p => p.PostDate);
            return View(posts.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}