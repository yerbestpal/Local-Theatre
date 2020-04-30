using AssessmentLocalTheatre.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net;
using AssessmentLocalTheatre.Extensions;

namespace AssessmentLocalTheatre.Controllers
{
    /// <summary>
    /// Manage post-related functionality.
    /// </summary>

    // Restrict controller access to Roles.
    [Authorize(Roles = "Author,Admin")]
    public class PostController : Controller
    {
        /// <summary>
        /// Instance of the database.
        /// </summary>
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        /// <summary>
        /// Returns list of all posts in system.
        /// </summary>
        /// <returns>list of all posts in system.</returns>
        public ActionResult ViewAllPosts()
        {
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff);
            return View(posts.ToList());
        }

        /// <summary>
        /// Approve individual posts.
        /// </summary>
        /// <param name="PostId">Posts ID.</param>
        /// <returns>Redirect to ViewAllPosts action in PostController.</returns>
        public ActionResult Approve(int? id)
        {
            Post post = context.Posts.Find(id);

            if (post == null)
            {
                this.AddNotification("Could not find post.", NotificationType.WARNING);
                return RedirectToAction("ViewAllPosts");
            }

            post.IsApproved = true;
            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("ViewAllPosts", post);
        }


        // GET: Posts
        /// <summary>
        /// Returns post history view for individual post authors.
        /// </summary>
        /// <returns>post history view and posts list as parameter.</returns>  
        public ActionResult Index()
        {
            var posts = context.Posts.Include(p => p.Category).Include(p => p.Staff);
            var userId = User.Identity.GetUserId();
            posts = posts.Where(p => p.StaffId == userId);

            return View(posts.ToList());
        }

        // GET: Post/Details/5
        /// <summary>
        /// This Details action is called when registered Staff clicks the link "Post Details".
        /// </summary>
        /// <param name="id">Nullable Post id.</param>
        /// <returns>View, including post found with id.</returns>
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Post post = context.Posts.Find(id);
            var author = context.Users.Find(post.StaffId);
            var category = context.Categories.Find(post.CategoryId);
            post.Staff = (Staff)author;
            post.Category = category;
            if (post == null) return HttpNotFound();

            List<Comment> comments = context.Comments.ToList();
            foreach (Comment comment in comments)
            {
                if (comment.PostId == post.PostId)
                {
                    ApplicationUser commentAuthor = context.Users.Find(comment.ApplicationUserId);
                    comment.ApplicationUser = commentAuthor;
                }
            }

            return View(post);
        }

        // GET: Staff/Create
        /// <summary>
        /// Returns Create Post view.
        /// </summary>
        /// <returns>Create Post View.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            try
            {
                ViewBag.CategoryId = new SelectList(context.Categories, "Categoryid", "Name");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                this.AddNotification("Error loading post Create view: " + ex, NotificationType.WARNING);
                return RedirectToAction("Index");
            }
        }

        // POST: Post/Create
        /// <summary>
        /// Create new post and return different views, depending on role.
        /// </summary>
        /// <param name="post">Post instance to be created.</param>
        /// <returns>Either redirect to /Staff/Index or /Admin/ViewAllPosts.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PostId,CategoryId,Title,Description,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.PostDate = DateTime.Now;
                post.IsApproved = false;
                post.StaffId = User.Identity.GetUserId();
                context.Posts.Add(post);
                context.SaveChanges();

                if (User.IsInRole("Author")) return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Post/Edit/5
        /// <summary>
        /// Passes existing post to the Edit view.
        /// </summary>
        /// <param name="id">Nullable post ID.</param>
        /// <returns>View and post instance parameter.</returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Post post = context.Posts.Find(id);

            if (post == null)
            {
                this.AddNotification("Cannot find post.", NotificationType.WARNING);
                return View(post);
            }

            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);
            return View(post);
        }

        // POST: Posts/edit/5
        /// <summary>
        /// Adds updated Post to database.
        /// </summary>
        /// <param name="post">Post to be edited.</param>
        /// <returns>Edit view, including edited post.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PostId,Title,Description,Content,CategoryId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.StaffId = User.Identity.GetUserId();
                post.PostDate = DateTime.Now;
                post.IsApproved = false;

                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();
                
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("ViewAllPosts");
                }
                else if (User.Identity.GetUserId().Equals(post.StaffId))
                {
                    return RedirectToAction("Index");
                }
            }
            ViewBag.CategoryId = new SelectList(context.Categories, "CategoryId", "Name", post.CategoryId);
            this.AddNotification("You must be the post author to edit this post.", NotificationType.WARNING);
            return RedirectToAction("ViewAllPosts");
        }

        // GET: Posts/Delete/5
        /// <summary>
        /// Return post Delete view.
        /// </summary>
        /// <param name="id">Nullable Post ID.</param>
        /// <returns>Post Delete view, including post as parameter.</returns>
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            Post post = context.Posts.Find(id);
            if (post == null) return HttpNotFound();
            var category = context.Categories.Find(post.CategoryId);
            post.Category = category;
            return View(post);
        }

        // Post: Posts/Delete/5
        /// <summary>
        /// Delete post and redirect to /Staff/Index.
        /// </summary>
        /// <param name="id">Post ID.</param>
        /// <returns>Redirect to /Staff/Index view.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = context.Posts.Find(id);
            context.Posts.Remove(post);

            // Get and delete all comments belonging to post.
            Comment comment = context.Comments.Find(id);
            if (comment != null) context.Comments.Remove(comment);

            context.SaveChanges();
            return RedirectToAction("ViewAllPosts");
        }
    }
}
