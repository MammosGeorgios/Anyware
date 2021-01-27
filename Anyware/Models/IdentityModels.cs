using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Anyware.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }
        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "A phone numbers must have 10 digits only")]
        [Display(Name = "Personal Phone")]
        public string PersonalPhone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }


        public int VendorID { get; set; }
        [ForeignKey("VendorID")]
        public virtual Vendor Vendor { get; set; }

        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class AppUsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppUsersDbContext()
            : base("SystemUsers", throwIfV1Schema: false)
        {
        }

        public static AppUsersDbContext Create()
        {
            return new AppUsersDbContext();
        }

        public System.Data.Entity.DbSet<Anyware.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.Vendor> Vendors { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.Invoice> Invoices { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.VatCategory> VatCategories { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.ProductCategory> ProductCategories { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.ProductUnitOfMeasurement> ProductUnitOfMeasurements { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Anyware.Models.ProductInOrder> ProductInOrders { get; set; }

        
    }
}