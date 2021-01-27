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
    public class VatCategoriesController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: VatCategories
        public async Task<ActionResult> Index()
        {
            return View(await db.VatCategories.ToListAsync());
        }

        // GET: VatCategories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VatCategory vatCategory = await db.VatCategories.FindAsync(id);
            if (vatCategory == null)
            {
                return HttpNotFound();
            }
            return View(vatCategory);
        }

        // GET: VatCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VatCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VatCategoryID,VatName,VatPercentage")] VatCategory vatCategory)
        {
            if (ModelState.IsValid)
            {
                db.VatCategories.Add(vatCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vatCategory);
        }

        // GET: VatCategories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VatCategory vatCategory = await db.VatCategories.FindAsync(id);
            if (vatCategory == null)
            {
                return HttpNotFound();
            }
            return View(vatCategory);
        }

        // POST: VatCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VatCategoryID,VatName,VatPercentage")] VatCategory vatCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vatCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vatCategory);
        }

        // GET: VatCategories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VatCategory vatCategory = await db.VatCategories.FindAsync(id);
            if (vatCategory == null)
            {
                return HttpNotFound();
            }
            return View(vatCategory);
        }

        // POST: VatCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VatCategory vatCategory = await db.VatCategories.FindAsync(id);
            db.VatCategories.Remove(vatCategory);
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
