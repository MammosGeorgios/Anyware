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

namespace Anyware.Controllers
{
    public class ProductInOrdersController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: ProductInOrders
        public async Task<ActionResult> Index()
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            var productInOrders = db.ProductInOrders.Include(p => p.Order).Include(p => p.Product);
            return View(await productInOrders.ToListAsync());
        }

        // GET: ProductInOrders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInOrder productInOrder = await db.ProductInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return HttpNotFound();
            }
            return View(productInOrder);
        }

       




        // GET: ProductInOrders/Create
        [HttpPost]
        public ActionResult Create([Bind(Include ="ProductID,Quantity")] ProductInOrderCreateModel viewModel)
        {
            ProductInOrder productInOrder = new ProductInOrder();
            productInOrder.ProductID = viewModel.ProductID;
            productInOrder.Quantity = viewModel.Quantity;

            string userID = User.Identity.GetUserId();
            Order activeOrder = db.Orders.FirstOrDefault(x => x.UserId == userID && x.OrderStatus == 0);
            if (activeOrder == null)
            {
                return RedirectToAction("Warning", "Home",new { message = "No active order currently. Click New order below if you are a customer to proceed to an order. " });
            }
            productInOrder.OrderID = activeOrder.OrderID;

            ProductInOrder p = db.ProductInOrders.FirstOrDefault(x => x.OrderID == activeOrder.OrderID && x.ProductID == productInOrder.ProductID);

            //If product is already in Order
            if(  p != null)
            {
                p.Quantity = viewModel.Quantity;
                db.SaveChanges();
                return RedirectToAction("Warning", "Home", new { message = "Product already in order, updated the amount order!" });
            }
           

            db.ProductInOrders.Add(productInOrder);
            db.SaveChanges();

            ViewBag.Message = "Product added to order";
            return RedirectToAction("Index", "Products");


        }

       

        // GET: ProductInOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInOrder productInOrder = await db.ProductInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserId", productInOrder.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productInOrder.ProductID);
            return View(productInOrder);
        }

        // POST: ProductInOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductInOrderID,ProductID,OrderID,Quantity")] ProductInOrder productInOrder)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            if (ModelState.IsValid)
            {
                db.Entry(productInOrder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserId", productInOrder.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productInOrder.ProductID);
            return View(productInOrder);
        }

        // GET: ProductInOrders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInOrder productInOrder = await db.ProductInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return HttpNotFound();
            }
            return View(productInOrder);
        }

        public async Task<ActionResult> Delete(int? id,int? orderID)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            if (id == null || orderID==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductInOrder productInOrder = await db.ProductInOrders.FindAsync(id);
            if (productInOrder == null)
            {
                return HttpNotFound();
            }
            return View(productInOrder);
        }

        // POST: ProductInOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Restricted to administrator only!" });
            }
            ProductInOrder productInOrder = await db.ProductInOrders.FindAsync(id);
            db.ProductInOrders.Remove(productInOrder);
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


        public async Task<ActionResult> RemoveFromOrder(int id, int orderID) {
            ProductInOrder productInOrder = await db.ProductInOrders.FindAsync(id);
            if (productInOrder.OrderID != orderID)
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - NON VALID REMOVE-FROM-ORDER REQUEST" });
            }

            db.ProductInOrders.Remove(productInOrder);
            await db.SaveChangesAsync();

            return RedirectToAction("OrderDetails", "Orders");

        }
    }
}
