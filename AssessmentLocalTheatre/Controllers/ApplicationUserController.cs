using AssessmentLocalTheatre.Extensions;
using AssessmentLocalTheatre.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        RoleManager<IdentityRole> roleManager;
        UserManager<ApplicationUser> userManager;

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

        // GET: Staff/Edit/5
        /// <summary>
        /// Loads Edit view.
        /// </summary>
        /// <param name="id">Staff id.</param>
        /// <returns>Edit view.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditStaff(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    Staff staff = context.Users.Find(id) as Staff;
                    if (staff == null) return HttpNotFound();

                    return View(staff);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading EditStaff view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("ViewAllStaff", "ApplicationUser");
                }
            }
            return RedirectToAction("ViewAllStaff", "ApplicationUser");
        }

        // POST: Staff/Edit/5
        /// <summary>
        /// Edit staff in database.
        /// </summary>
        /// <param name="id">Staff id.</param>
        /// <returns>Edit view.</returns>
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditStaff(Staff staff)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (staff == null) return HttpNotFound();
                    staff.UserName = staff.Email;
                    context.Entry(staff).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("ViewAllStaff");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading EditStaff view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("ViewAllStaff", "ApplicationUser");
                }
            }
            return RedirectToAction("ViewAllStaff", "ApplicationUser");
        }

        // GET: ApplicationUser/Edit/5
        /// <summary>
        /// Loads Edit view.
        /// </summary>
        /// <param name="id">ApplicationUser id.</param>
        /// <returns>Edit view.</returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditUser(string id)
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
                    this.AddNotification("Error loading EditUser view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("ViewAllMembers");
                }
            }
            return RedirectToAction("ViewAllMembers");
        }

        // POST: Staff/Edit/5
        /// <summary>
        /// Edit ApplicationUser in database.
        /// </summary>
        /// <param name="id">ApplicationUser id.</param>
        /// <returns>Edit view.</returns>
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditUser(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (user == null) return HttpNotFound();
                    user.UserName = user.Email;
                    context.Entry(user).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("ViewAllMembers");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading EditUser view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("ViewAllMembers", "ApplicationUser");
                }
            }
            return RedirectToAction("ViewAllStaff", "ApplicationUser");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> ChangeRole(string id)
        {

        }
    }
}
