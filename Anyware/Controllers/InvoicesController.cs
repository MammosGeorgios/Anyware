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
using Anyware.Hubs;
using Microsoft.AspNet.Identity;

namespace Anyware.Controllers
{
    public class InvoicesController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: Invoices
        public async Task<ActionResult> Index(string sortOrder)
        {
            
            IQueryable<Invoice> invoices; 

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else if(User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "User not verified with a vendor. Please contact us for more information on how to verify your account" });
            }
            else if (User.IsInRole("Validated User"))
            {
                string userId = User.Identity.GetUserId();
                int activeUserId = db.Users.FirstOrDefault(x=>x.Id==userId).VendorID;
                invoices = db.Invoices.Include(i => i.Order).Where(i => i.Order.ApplicationUser.VendorID == activeUserId);
            }
            else //if(User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                invoices = db.Invoices.Include(i => i.Order).OrderByDescending(x=>x.DateCreated);
            }

            
        
             ViewBag.Message = "?sortOrder=" + sortOrder;
            return View(await invoices.ToListAsync());
        }

        // GET: Invoices/Details/5
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
            var invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }


            // NEED TO MAKE IT SO IF USER HAS ACCESS TO THIS INVOICE FROM HIS VENDOR HE CAN SEE, OTHERWISE HE CANNOT
            string userId = User.Identity.GetUserId();
            ApplicationUser user = db.Users.FirstOrDefault(x=>x.Id==userId);
            if((invoice.Order.ApplicationUser.VendorID != user.VendorID) && !(User.IsInRole("Administrator") || User.IsInRole("Manager")))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - You are trying to view an invoice belonging to another Vendor" });
            }

            // PULL ORDER DETAILS *****************************************************

            Order order = invoice.Order;
            var productsInOrder = db.ProductInOrders.Where(x => x.OrderID == order.OrderID);
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
            //*******************************************************************************************
            // Make viewmodel and return it
            InvoiceDetails invoiceDetails = new InvoiceDetails();
            invoiceDetails.Invoice = invoice;
            invoiceDetails.OrderDetails = od;

            return View(invoiceDetails);
        }

       

        public async Task<ActionResult> Create(int? orderID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Order order = await db.Orders.FindAsync(orderID);
            if (order == null)
            {
                return HttpNotFound();
            }

            var products = db.ProductInOrders.Where(x => x.OrderID == order.OrderID).Include(x => x.Product.ProductCategory.VatCategory);
            if (products.Count() == 0)
            {
                return RedirectToAction("Warning", "Home", new { message = "No products added in this order. Click Continue Order to add products." });
            }
            // Check if products are available!
            if (!UpdateProducts(order)){ 
            
                return RedirectToAction("Warning", "Home", new { message = "Not all products are available. Click View Order to see details. " });
            }

            decimal totalPrice = 0;
            
            foreach (var item in products )
            {
                totalPrice += (item.Product.ProductPrice + Convert.ToDecimal( item.Product.ProductCategory.VatCategory.VatPercentage)  * item.Product.ProductPrice /100.0m)*item.Quantity;
            }

            // If products are available, reduce all stocks then complete the rest

            order.OrderStatus = (OrderStatus)1;

            Invoice invoice = new Invoice();
            invoice.OrderID = (int)orderID;
            invoice.DateCreated = DateTime.Now;
            invoice.PaymentDueDate = DateTime.Now.AddMonths(2);
            invoice.InvoiceStatus = (InvoiceStatus)0;
            invoice.TotalPrice = totalPrice;
            db.Invoices.Add(invoice);

            await db.SaveChangesAsync();
            InvoicesHub.BroadcastData();
            return (RedirectToAction("Index"));


        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InvoiceID,OrderID,InvoiceStatus,PaymentDueDate,DateCreated")] Invoice invoice)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                await db.SaveChangesAsync();
                InvoicesHub.BroadcastData();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserId", invoice.OrderID);
            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserId", invoice.OrderID);
            return View(invoice);
        }


        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InvoiceID,OrderID,InvoiceStatus,PaymentDueDate,DateCreated,TotalPrice")] Invoice invoice)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                InvoicesHub.BroadcastData();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserId", invoice.OrderID);
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            Invoice invoice = await db.Invoices.FindAsync(id);
            db.Invoices.Remove(invoice);
            await db.SaveChangesAsync();
            InvoicesHub.BroadcastData();
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

        public async Task<ActionResult> ConfirmShipment (int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            invoice.InvoiceStatus = (InvoiceStatus)1;
            db.Entry(invoice).State = EntityState.Modified;
            await db.SaveChangesAsync();
            InvoicesHub.BroadcastData();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ConfirmPayment (int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            invoice.InvoiceStatus = (InvoiceStatus)2;
            db.Entry(invoice).State = EntityState.Modified;
            await db.SaveChangesAsync();
            InvoicesHub.BroadcastData();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Cancel(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            invoice.InvoiceStatus = (InvoiceStatus)3;
            
            ReplenishProducts(invoice.Order);

            await db.SaveChangesAsync();
            ProductsHub.BroadcastData();

            return RedirectToAction("Index");
        }
        private bool UpdateProducts(Order activeOrder)
        {

            // start by getting all productInOrder data
            var productInOrders = db.ProductInOrders.Where(x => x.OrderID == activeOrder.OrderID).Include(x => x.Product);

          
            // check if all products are available in the quantities needed
            bool allProductsAvailable = true;
            foreach (var item in productInOrders)
            {
                if (item.Quantity > item.Product.UnitsInStock)
                    allProductsAvailable = false;                
            }

            if (!allProductsAvailable)
                return (false);  //if a single product isn't available we go back to the action with a false

            // Now i want to update all products with new quantities 
            
            foreach (var item in productInOrders)
            {
                Product product = item.Product;
                product.UnitsInStock -= item.Quantity;
                if (product.UnitsInStock == 0) product.ProductStatus = (ProductStatus)1;

               
            }

            db.SaveChanges();
            ProductsHub.BroadcastData();

            return (true);



        }

        public void ReplenishProducts(Order order)
        {
            // start by getting all productInOrder data
            var productInOrders = db.ProductInOrders.Where(x => x.OrderID == order.OrderID).Include(x => x.Product);


            foreach (var item in productInOrders)
            {
                Product product = item.Product;
                if (product.UnitsInStock == 0) product.ProductStatus = (ProductStatus)0;
                product.UnitsInStock += item.Quantity;
                

            }

            db.SaveChanges();
            ProductsHub.BroadcastData();

        }


        public ActionResult GetInvoiceData(string sortOrder)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            IQueryable<Invoice> invoices; //= db.Invoices.Include(i => i.Order);

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "User not verified with a vendor. Please contact us for more information on how to verify your account" });
            }
            else if (User.IsInRole("Validated User"))
            {
                string userId = User.Identity.GetUserId();
                int activeUserId = db.Users.FirstOrDefault(x => x.Id == userId).VendorID;
                invoices = db.Invoices.Include(i => i.Order).Where(i => i.Order.ApplicationUser.VendorID == activeUserId);
            }
            else //if(User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                invoices = db.Invoices.Include(i => i.Order).OrderByDescending(x => x.DateCreated);
            }

            #region sorts
            ViewData["VendorNameParam"] = String.IsNullOrEmpty(sortOrder) ? "VendorName_desc" : "VendorName_asc"; // Den to theloume logika - mammos
            ViewData["InvoiceStatusParam"] = String.IsNullOrEmpty(sortOrder) ? "InvoiceStatus_desc" : "InvoiceStatus_asc";
            ViewData["DateSortParam"] = String.IsNullOrEmpty(sortOrder) ? "PaymentDueDate_desc" : "PaymentDueDate_asc";
            ViewData["DateCreatedParam"] = String.IsNullOrEmpty(sortOrder) ? "DateCreated_desc" : "DateCreated_asc";
            if (!String.IsNullOrEmpty(sortOrder))
            {

                switch (sortOrder)
                {
                    case "VendorName_asc":
                        ViewData["VendorNameParam"] = "VendorName_desc";
                        break;
                    case "InvoiceStatus_asc":
                        ViewData["InvoiceStatusParam"] = "InvoiceStatus_desc";
                        break;
                    case "PaymentDueDate_asc":
                        ViewData["DateSortParam"] = "PaymentDueDate_desc";
                        break;
                    case "DateCreated_asc":
                        ViewData["DateCreatedParam"] = "DateCreated_desc";
                        break;
                    default:
                        break;
                }
            }

            switch (sortOrder)
            {
                case "VendorName_desc":
                    invoices = invoices.OrderByDescending(i => i.Order.ApplicationUser.Vendor.VendorName);
                    break;
                case "VendorName_asc":
                    invoices = invoices.OrderBy(i => i.Order.ApplicationUser.Vendor.VendorName);
                    break;
                case "InvoiceStatus_desc":
                    invoices = invoices.OrderByDescending(i => i.InvoiceStatus);
                    break;
                case "InvoiceStatus_asc":
                    invoices = invoices.OrderBy(i => i.InvoiceStatus);
                    break;
                case "PaymentDueDate_desc":
                    invoices = invoices.OrderByDescending(i => i.PaymentDueDate);
                    break;
                case "PaymentDueDate_asc":
                    invoices = invoices.OrderBy(i => i.PaymentDueDate);
                    break;
                case "DateCreated_desc":
                    invoices = invoices.OrderByDescending(i => i.DateCreated);
                    break;
                case "DateCreated_asc":
                    invoices = invoices.OrderBy(i => i.DateCreated);
                    break;
            }

            #endregion
            return PartialView("_InvoiceData", invoices.ToList());
        }

    }
}
