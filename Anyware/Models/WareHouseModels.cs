using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    

    public class ProductsPlusProductInOrder
    {
        public IEnumerable<Product> Products { get; set; }
        public ProductInOrder ProductInOrder { get; set; }
    }

    public class OrderDetails
    {
        public Order Order { get; set; }
        public IList<ProductInOrderDetails> ProductInOrderDetails { get; set; }
    }

    public class ProductInOrderDetails
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public bool Available { get; set; }
        public int ProductInOrderID { get; set; }
    }

    public class ProductFilters
    {
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
        public bool? InStock { get; set; } 
    }

   public class ProductInOrderCreateModel
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        
    }

   public class ProductModel
    {
        
        [Display(Name = "ID")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        
        [Display(Name = "Status")]
        public ProductStatus ProductStatus { get; set; }
       
        public  ProductCategory ProductCategory { get; set; }


        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        public  ProductUnitOfMeasurement ProductUnitOfMeasurement { get; set; }

        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        public int UnitsToBuy { get; set; }
    }

    public class InvoiceDetails
    {
        public Invoice Invoice { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }

    public class VendorDetails
    {
        public Vendor Vendor { get; set; }

        public IList<UserBasicDetails> Users { get; set; }

    }

    public class UserBasicDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }

    public class UserAdminInfo
    {
        public ApplicationUser User { get; set; }

        public List<string> Roles { get;set; }


    }
}