using AssessmentLocalTheatre.Extensions;
using AssessmentLocalTheatre.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AssessmentLocalTheatre.Controllers
{
    /// <summary>
    /// Main Category controller.
    /// Contains all methods for performing CRUD functions on the Category class.
    /// </summary>

    // Restrict controller access to Roles.
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        // Instance of the database.
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        /// GET: Category
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
                    var categories = context.Categories;
                    return View(categories.ToList());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading Index view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Category/Create
        /// <summary>
        /// Loads the Create view.
        /// </summary>
        /// <returns>Create view.</returns>
        public ActionResult Create()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return View();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading create category view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Category/Create
        /// <summary>
        /// Add Category to database.
        /// </summary>
        /// <param name="category">Category instance.</param>
        /// <returns>Index view.</returns>
        [HttpPost]
        public ActionResult Create([Bind(Include = "CategoryId,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Categories.Add(category);
                    context.SaveChanges();

                    if (User.IsInRole("Author")) return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error creating new category: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("", "Error creating new category.");
            return RedirectToAction("Index");
        }

        // GET: Category/Edit/5
        /// <summary>
        /// Load the Edit view.
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <returns>Edit view.</returns>
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                    Category category = context.Categories.Find(id);
                    if (category == null)
                    {
                        this.AddNotification("Cannot find category.", NotificationType.WARNING);
                        return View(category);
                    }
                    return View(category);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading edit category view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Category/Edit/5
        /// <summary>
        /// Update Category in database.
        /// </summary>
        /// <param name="post">Category instance.</param>
        /// <returns>Index view.</returns>
        [HttpPost]
        public ActionResult Edit([Bind(Include = "CategoryId, Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    context.Entry(category).State = EntityState.Modified;
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error editing category: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Category/Delete/5
        /// <summary>
        /// Load the Delete view.
        /// </summary>
        /// <param name="id">Category Id.</param>
        /// <returns>Delete view.</returns>
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                    Category category = context.Categories.Find(id);
                    if (category == null) return HttpNotFound();
                    return View(category);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error loading deleting view: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        // POST: Category/Delete/5
        /// <summary>
        /// Delete Category in database.
        /// </summary>
        /// <param name="post">Category instance.</param>
        /// <returns>Index view.</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Category category = context.Categories.Find(id);
                    context.Categories.Remove(category);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    this.AddNotification("Error deleting category: " + ex, NotificationType.WARNING);
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
