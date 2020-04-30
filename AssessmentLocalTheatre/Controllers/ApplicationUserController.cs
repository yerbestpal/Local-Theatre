using AssessmentLocalTheatre.Extensions;
using AssessmentLocalTheatre.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AssessmentLocalTheatre.Controllers
{
    /// <summary>
    /// Main ApplicationUser controller.
    /// Contains all methods for performing CRUD functions on the ApplicationUser class.
    /// </summary>

    // Restrict controller access to Roles.
    //[Authorize(Roles = "Admin")]
    public class ApplicationUserController : Controller
    {
        // Instance of the database.
        private ApplicationDbContext context = new ApplicationDbContext();

        // GET: ApplicationUser
        /// <summary>
        /// Loads the Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        public ActionResult Index()
        {
            if (ModelState.IsValid)
            {
                try
                {

                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }

        // GET: ApplicationUser
        /// <summary>
        /// Loads the ViewAllStaff view.
        /// </summary>
        /// <returns>ViewAllStaff view.</returns>
        public ActionResult ViewAllStaff()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // All authors and admins are staff.
                    var staff = context.Users.OfType<Staff>().ToList();
                    return View(staff.ToList());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading ViewAllStaff view: " + ex, NotificationType.WARNING);
                    return View();
                }
            }
            return View();
        }

        // GET: ApplicationUser
        /// <summary>
        /// Loads the ViewAllMembers view.
        /// </summary>
        /// <returns>ViewAllMembers view.</returns>
        public ActionResult ViewAllMembers()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var memberRole = context.Roles.Where(r => r.Name == "Member").SingleOrDefault();
                    var members = context.Users.Where(u => u.Roles.Any(r => r.RoleId == memberRole.Id));

                    return View(members.ToList());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading ViewAllMembers view: " + ex, NotificationType.WARNING);
                    return View();
                }
            }
            return View();
        }

        // GET: ApplicationUser/Details/5
        /// <summary>
        /// Loads Details view.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Details view.</returns>
        public ActionResult Details(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    ApplicationUser user = context.Users.Find(id);
                    if (user == null) return HttpNotFound();
                    return View(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading Delete view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Index", "Admin");
        }



        // GET: ApplicationUser/Delete/5
        /// <summary>
        /// Loads Delete view.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Details view.</returns>
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    ApplicationUser user = context.Users.Find(id);
                    if (user == null) return HttpNotFound();
                    return View(user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading Delete view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index", "Admin");
                }
            }
            return RedirectToAction("Index", "Admin");
        }

        // POST: ApplicationUser/Delete/5
        /// <summary>
        /// Remove user from database.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Details view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = context.Users.Find(id);
                    context.Users.Remove(user);
                    foreach (Post post in context.Posts)
                    {
                        if (post.StaffId.Equals(id)) context.Posts.Remove(post);
                    }
                    foreach (Comment comment in context.Comments)
                    {
                        if (comment.ApplicationUserId.Equals(id)) context.Comments.Remove(comment);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Index", "Admin");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error removing user from database: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Details", "ApplicationUser");
                }
            }
            return RedirectToAction("Details", "ApplicationUser");
        }
    }
}
