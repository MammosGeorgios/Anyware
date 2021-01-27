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
using Anyware.Tools;
using Microsoft.AspNet.Identity;

namespace Anyware.Controllers
{
    // return RedirectToAction("Warning", "Home", new { message = "User not verified with a vendor. Please contact us for more information on how to verify your account" });
public class VendorsController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();

        // GET: Vendors
        public async Task<ActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }


            return View(await db.Vendors.ToListAsync());
        }

        // GET: Vendors/Details/5
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

            //Check if user belongs to this vendor. If not, deny access

            string userId = User.Identity.GetUserId();
            var user =await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            int userVendorID = user.VendorID;
            if (userVendorID != id && !( User.IsInRole("Administrator")||User.IsInRole("Manager")))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - You can't view other Vendor Details" });
            }

            Vendor vendor = await db.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }

            // Find all users with this vendor

            var users = db.Users.Where(x => x.VendorID == id);

            // Create view model to return

            VendorDetails vd = new VendorDetails();
            vd.Vendor = vendor;
            vd.Users = new List<UserBasicDetails>();
            foreach (var item in users)
            {
                UserBasicDetails temp = new UserBasicDetails
                {

                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Email = item.Email,
                    Phone = item.PersonalPhone

                };
                vd.Users.Add(temp);
                
            }

            return View(vd);
        }

        // GET: Vendors/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (User.IsInRole("Validated User")||User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VendorID,VendorName,VendorAFM,VendorLegalName,VendorDOI")] Vendor vendor)
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
                vendor.VendorSecretKey = KeyGenerator.GetUniqueKey(66);
                db.Vendors.Add(vendor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (User.IsInRole("Validated User") || User.IsInRole("Unvalidated User"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED" });
            }
            Vendor vendor = await db.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VendorID,VendorName,VendorAFM,VendorLegalName,VendorDOI")] Vendor vendor)
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
                db.Entry(vendor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        // GET: Vendors/Delete/5
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
            Vendor vendor = await db.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: Vendors/Delete/5
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
            Vendor vendor = await db.Vendors.FindAsync(id);
            db.Vendors.Remove(vendor);
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
