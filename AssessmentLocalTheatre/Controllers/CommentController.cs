using AssessmentLocalTheatre.Extensions;
using AssessmentLocalTheatre.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssessmentLocalTheatre.Controllers
{
    /// <summary>
    /// Controller for Comment class.
    /// Manages operations on Comments.
    /// </summary>
    public class CommentController : Controller
    {
        // Instance of the database.
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "CommentId, ApplicationUserId, Content, Date, IsAuthorised, PostId")] Comment comment, int Id)
        {
            // Get users ID.
            string userId = User.Identity.GetUserId();

            // Redirect user to login page if ID is null.
            if (userId == null)
            {
                // Using bootstrapNotifications package to send error message from controller to view.
                this.AddNotification("Please login.", NotificationType.WARNING);
                return RedirectToAction("Login", "Account");
            }

            // Find user in database.
            ApplicationUser loggedInUser = context.Users.Find(userId);

            // Check is user is suspended.
            if (loggedInUser.IsSuspended)
            {
                // Using bootstrapNotifications package to send error message from controller to view.
                this.AddNotification("Your account is suspended.", NotificationType.WARNING);
                return PartialView(comment);
            }

            // Return Comment view if no content is present.
            if (comment.Content == null || comment.Content == "")
            {
                this.AddNotification("You cannot post blank comments.", NotificationType.WARNING);
                return PartialView(comment);
            }

            // Create Comment.
            Post ParentPost = context.Posts.Find(Id);
            comment.Post = ParentPost;
            comment.PostId = Id;
            comment.ApplicationUser = loggedInUser;
            comment.Date = DateTime.Now;

            // Add Comment to database.
            context.Comments.Add(comment);

            // Save database changes.
            context.SaveChanges();

            //Page.Response.Redirect(Page.Request.Url.ToString(), true);
            //Response.Redirect(Request.RawUrl);


            // Return to Post.
            return PartialView(comment);



            //if (ModelState.IsValid)
            //{

            //}
            //else
            //{
            //    return PartialView(comment);
            //}
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id) 
        {
            return View();
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
