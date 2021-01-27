using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Anyware.Models;

namespace Anyware.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: ProductCategories
        public async Task<ActionResult> Index()
        {
            var productCategories = db.ProductCategories.Include(p => p.VatCategory);
            return View(await productCategories.ToListAsync());
        }

        // GET: ProductCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = await db.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }

            ViewBag.VatCategoryID = new SelectList(db.VatCategories, "VatCategoryID", "VatName");
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductCategoryID,CategoryName,VatCategoryID")] ProductCategory productCategory)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }

            if (ModelState.IsValid)
            {
                db.ProductCategories.Add(productCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.VatCategoryID = new SelectList(db.VatCategories, "VatCategoryID", "VatName", productCategory.VatCategoryID);
            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            ProductCategory productCategory = await db.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.VatCategoryID = new SelectList(db.VatCategories, "VatCategoryID", "VatName", productCategory.VatCategoryID);
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductCategoryID,CategoryName,VatCategoryID")] ProductCategory productCategory)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (ModelState.IsValid)
            {
                db.Entry(productCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.VatCategoryID = new SelectList(db.VatCategories, "VatCategoryID", "VatName", productCategory.VatCategoryID);
            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = await db.ProductCategories.FindAsync(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            ProductCategory productCategory = await db.ProductCategories.FindAsync(id);
            db.ProductCategories.Remove(productCategory);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}
