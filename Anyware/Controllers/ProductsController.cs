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
using Microsoft.AspNet.Identity;
using Anyware.Hubs;

namespace Anyware.Controllers
{
    public class ProductsController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: Products
        
        public async Task<ActionResult> Index(string searchString, bool? inStock, string sortOrder)
        {
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
           

            ViewBag.Message = "?";
            if (sortOrder != null) ViewBag.Message += "sortOrder=" + sortOrder;
            if (inStock == true) ViewBag.Message += "&inStock=" + true;
            if (!String.IsNullOrEmpty(searchString)) ViewBag.Message += "&searchString=" + searchString;



            ProductFilters pf = new ProductFilters();
            pf.InStock = inStock;
            pf.SearchString = searchString;
            pf.SortOrder = sortOrder;

            return View(pf);
        }
        [HttpPost]
        public async Task<ActionResult> Index (string searchString, bool? inStock )
        {
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
            ProductFilters pf = new ProductFilters();
            // need viewbag message for the filter to show up atm

            
                ViewBag.Message += "?searchString=" + searchString;
                ViewBag.Message += "&inStock=True";
           

            pf.SearchString = searchString;
            pf.InStock = inStock;            
            pf.SortOrder = "";

            return View(pf);

        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ProductCategoryID", "CategoryName");
            ViewBag.ProductUnitOfMeasurementID = new SelectList(db.ProductUnitOfMeasurements, "ProductUnitOfMeasurementID", "UnitName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductID,ProductName,Description,ProductStatus,ProductCategoryID,ProductPrice,ProductUnitOfMeasurementID,UnitsInStock")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductStatus < (ProductStatus)2)
                {
                    product.ProductStatus = (product.UnitsInStock > 0) ? (ProductStatus)0 : (ProductStatus)1;
                }
                db.Products.Add(product);
                
                await db.SaveChangesAsync();
                ProductsHub.BroadcastData();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ProductCategoryID", "CategoryName", product.ProductCategoryID);
            ViewBag.ProductUnitOfMeasurementID = new SelectList(db.ProductUnitOfMeasurements, "ProductUnitOfMeasurementID", "UnitName", product.ProductUnitOfMeasurementID);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ProductCategoryID", "CategoryName", product.ProductCategoryID);
            ViewBag.ProductUnitOfMeasurementID = new SelectList(db.ProductUnitOfMeasurements, "ProductUnitOfMeasurementID", "UnitName", product.ProductUnitOfMeasurementID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductID,ProductName,Description,ProductStatus,ProductCategoryID,ProductPrice,ProductUnitOfMeasurementID,UnitsInStock")] Product product)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                product.ProductStatus = (product.UnitsInStock > 0) ? (ProductStatus)0 : (ProductStatus)1;
                await db.SaveChangesAsync();
                ProductsHub.BroadcastData();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryID = new SelectList(db.ProductCategories, "ProductCategoryID", "CategoryName", product.ProductCategoryID);
            ViewBag.ProductUnitOfMeasurementID = new SelectList(db.ProductUnitOfMeasurements, "ProductUnitOfMeasurementID", "UnitName", product.ProductUnitOfMeasurementID);
            return View(product);
        }

        // GET: Products/Delete/5
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
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            ProductsHub.BroadcastData();
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

        public ActionResult GetProductData(string searchString, bool? inStock, string sortOrder)
        {
            
            #region Sort and Filters + creating product ICollection
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "name_asc";
            ViewData["PriceSortParm"] = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "price_asc";
            ViewData["ProductStatusSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ProductStatus_desc" : "ProductStatus_asc";
            ViewData["CategoryName"] = String.IsNullOrEmpty(sortOrder) ? "CategoryName_desc" : "CategoryName_asc";
            ViewData["ProductUnitOfMeasurement"] = String.IsNullOrEmpty(sortOrder) ? "ProductUnitOfMeasurement_desc" : "ProductUnitOfMeasurement_asc";
            ViewData["Description"] = String.IsNullOrEmpty(sortOrder) ? "Description_desc" : "Description_asc";

            var products = db.Products.Include(p => p.ProductCategory).Include(p => p.ProductUnitOfMeasurement);
            var categories = db.ProductCategories.Include(c => c.CategoryName);

            // Setting the sorts so that they can be clicked multiple times in a row and keep switching
            if (!String.IsNullOrEmpty(sortOrder))
            {
                switch (sortOrder)
                {
                    case "name_asc":
                        ViewData["NameSortParm"] = "name_desc";
                        break;
                    case "price_asc":
                        ViewData["PriceSortParm"] = "price_desc";
                        break;
                    case "ProductStatus_asc":
                        ViewData["ProductStatusSortParm"] = "ProductStatus_desc";
                        break;
                    case "CategoryName_asc":
                        ViewData["CategoryName"] = "CategoryName_desc";
                        break;
                    case "ProductUnitOfMeasurement_asc":
                        ViewData["ProductUnitOfMeasurement"] = "ProductUnitOfMeasurement_desc";
                        break;
                    case "Description_asc":
                        ViewData["Description"] = "Description_desc";
                        break;
                    default:
                        break;
                }
            }


            //Sort Switch
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.ProductName);
                    break;
                case "name_asc":
                    products = products.OrderBy(p => p.ProductName);
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.ProductPrice);
                    break;
                case "price_asc":
                    products = products.OrderBy(p => p.ProductPrice);
                    break;
                case "productStatus_desc":
                    products = products.OrderByDescending(p => p.ProductStatus);
                    break;
                case "productStatus_asc":
                    products = products.OrderBy(p => p.ProductStatus);
                    break;
                case "CategoryName_desc":
                    products = products.OrderByDescending(p => p.ProductCategory.CategoryName);
                    break;
                case "CategoryName_asc":
                    products = products.OrderBy(p => p.ProductCategory.CategoryName);
                    break;
                case "ProductUnitOfMeasurement_desc":
                    products = products.OrderByDescending(p => p.ProductUnitOfMeasurement.ProductUnitOfMeasurementID);
                    break;
                case "ProductUnitOfMeasurement_asc":
                    products = products.OrderBy(p => p.ProductUnitOfMeasurement.ProductUnitOfMeasurementID);
                    break;
                case "Description_desc":
                    products = products.OrderByDescending(p => p.Description);
                    break;
                case "Description_asc":
                    products = products.OrderBy(p => p.Description);
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(x => x.ProductName.Contains(searchString) ||
                                               x.ProductCategory.CategoryName.Contains(searchString));
            }
            if (inStock == true)
            {
                products = products.Where(x => x.ProductStatus == 0);
            }
            #endregion

           


            return PartialView("_ProductData", products.ToList());
        }

      
    }
}
