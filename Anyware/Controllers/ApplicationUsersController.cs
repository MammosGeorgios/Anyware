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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace Anyware.Controllers
{
    //[Authorize(Roles ="Administrator")]
    public class ApplicationUsersController : Controller
    {
        private AppUsersDbContext db = new AppUsersDbContext();
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: ApplicationUsers
        public async Task<ActionResult> Index()
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Adminstrator Only " });
            }
            List<UserAdminInfo> userAdminInfo = new List<UserAdminInfo>();

            var applicationUsers = db.Users.Include(a => a.Vendor);

            foreach (var item in applicationUsers)
            {
                var roles = await UserManager.GetRolesAsync(item.Id);
                UserAdminInfo temp = new UserAdminInfo
                {
                    User = item,
                    Roles = roles.ToList()
                };

                userAdminInfo.Add(temp);
            }


            return View(userAdminInfo);
        }

        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Adminstrator Only " });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await db.Users.FirstAsync(x => x.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

       
        
        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Adminstrator Only " });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await db.Users.FirstAsync(x=>x.Id ==id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "VendorName", applicationUser.VendorID);
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,MiddleName,LastName,DateofBirth,PersonalPhone,VendorID,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Adminstrator Only " });
            }
            if (ModelState.IsValid)
            {
                db.Entry(applicationUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.VendorID = new SelectList(db.Vendors, "VendorID", "VendorName", applicationUser.VendorID);
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Adminstrator Only " });
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await db.Users.FirstAsync(x => x.Id == id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = await db.Users.FirstAsync(x => x.Id == id);
            db.Users.Remove(applicationUser);
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

        public async Task<ActionResult> ChangeRole (string userId,string role)
        {
            var roleStore = new RoleStore<IdentityRole>(new AppUsersDbContext());
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("Warning", "Home", new { message = "ACCESS DENIED - Adminstrator Only " });
            }

            // Clear all roles from user
            await UserManager.RemoveFromRoleAsync(userId, "Unvalidated User");
            await UserManager.RemoveFromRoleAsync(userId, "Validated User");
            await UserManager.RemoveFromRoleAsync(userId, "Manager");
           

            await roleManager.CreateAsync(new IdentityRole(role));

            //await UserManager.AddToRoleAsync(user.Id, "Validated User");
            await HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().AddToRoleAsync(userId, role);

            db.SaveChanges();
            ApplicationUser user = db.Users.FirstOrDefault(x => x.Id == userId);
            //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);


            return RedirectToAction("Index", "ApplicationUsers");
        }

    }
}
