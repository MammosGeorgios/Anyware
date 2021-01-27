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
    public class OrdersController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();
        private UserManager<ApplicationUser> userManager;

        // GET: Orders
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            var orders = db.Orders.Include(o => o.ApplicationUser);
            return View(await orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            string userId = User.Identity.GetUserId();
            if(order.UserId != userId && !User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }

            if (order == null)
            {
                return HttpNotFound();
            }

            var productsInOrder = db.ProductInOrders.Where(x => x.OrderID == id);

            OrderDetails od = new OrderDetails();
            od.Order = order;

            List<ProductInOrderDetails> productInOrderDetails = new List<ProductInOrderDetails>();
            foreach (var item in productsInOrder.ToList())
            {
                var temp = new ProductInOrderDetails();
                temp.ProductName = item.Product.ProductName;
                temp.Quantity = item.Quantity;
                temp.ProductInOrderID = item.ProductInOrderID;

                // Need to check if it's still available
                temp.Available = (temp.Quantity > item.Product.UnitsInStock) ? false : true;

                productInOrderDetails.Add(temp);
            }

            od.ProductInOrderDetails = productInOrderDetails;

            return View(od);
        }

      

        //Jan16th New Order Edit


        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
            // Makes sure older Order is cancelled
            string userID = User.Identity.GetUserId();
            Order previousOrder = db.Orders.FirstOrDefault(x => x.UserId == userID && x.OrderStatus == 0);
            if(previousOrder!= null)
            {
                previousOrder.OrderStatus = (OrderStatus)2;
            }
           

            // Create new Order
            Order order = new Order();
            order.DateStarted = DateTime.Now;
            order.UserId = User.Identity.GetUserId();
            order.OrderStatus = 0;
            db.Orders.Add(order);
            db.SaveChanges();
            return RedirectToAction("Index", "Products");

        }


        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "OrderID,UserId,DateStarted")] Order order)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "OrderID,UserId,DateStarted")] Order order)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
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

        public async Task<ActionResult> OrderDetails()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
            string userID = User.Identity.GetUserId();
            Order activeOrder =await  db.Orders.FirstOrDefaultAsync(x => x.UserId == userID && x.OrderStatus == 0);
            if (activeOrder == null)
            {
                return RedirectToAction("Warning", "Home", new { message = "No active Order! Please click 'New Order' below." });
            }

            return RedirectToAction("Details", "Orders", new { id = activeOrder.OrderID });
        }

        public async Task<ActionResult> CancelOrder(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            order.OrderStatus = (OrderStatus)2;
            await db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ContinueOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Verify with a vendor first!" });
            }
            // Check if there is an active order for this user
            string userID = User.Identity.GetUserId();
            Order previousOrder = db.Orders.FirstOrDefault(x => x.UserId == userID && x.OrderStatus == 0);
            if (previousOrder != null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return RedirectToAction("Warning", "Home", new { message = "No active Order! Please click 'New Order' below." });
            }
        }
    }
}
