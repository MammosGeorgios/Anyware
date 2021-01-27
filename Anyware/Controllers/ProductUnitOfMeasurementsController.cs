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
    public class ProductUnitOfMeasurementsController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: ProductUnitOfMeasurements
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductUnitOfMeasurements.ToListAsync());
        }

        // GET: ProductUnitOfMeasurements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductUnitOfMeasurement productUnitOfMeasurement = await db.ProductUnitOfMeasurements.FindAsync(id);
            if (productUnitOfMeasurement == null)
            {
                return HttpNotFound();
            }
            return View(productUnitOfMeasurement);
        }

        // GET: ProductUnitOfMeasurements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductUnitOfMeasurements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductUnitOfMeasurementID,UnitName,UnitAbbreviation")] ProductUnitOfMeasurement productUnitOfMeasurement)
        {
            if (ModelState.IsValid)
            {
                db.ProductUnitOfMeasurements.Add(productUnitOfMeasurement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productUnitOfMeasurement);
        }

        // GET: ProductUnitOfMeasurements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductUnitOfMeasurement productUnitOfMeasurement = await db.ProductUnitOfMeasurements.FindAsync(id);
            if (productUnitOfMeasurement == null)
            {
                return HttpNotFound();
            }
            return View(productUnitOfMeasurement);
        }

        // POST: ProductUnitOfMeasurements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductUnitOfMeasurementID,UnitName,UnitAbbreviation")] ProductUnitOfMeasurement productUnitOfMeasurement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productUnitOfMeasurement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productUnitOfMeasurement);
        }

        // GET: ProductUnitOfMeasurements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductUnitOfMeasurement productUnitOfMeasurement = await db.ProductUnitOfMeasurements.FindAsync(id);
            if (productUnitOfMeasurement == null)
            {
                return HttpNotFound();
            }
            return View(productUnitOfMeasurement);
        }

        // POST: ProductUnitOfMeasurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ProductUnitOfMeasurement productUnitOfMeasurement = await db.ProductUnitOfMeasurements.FindAsync(id);
            db.ProductUnitOfMeasurements.Remove(productUnitOfMeasurement);
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
