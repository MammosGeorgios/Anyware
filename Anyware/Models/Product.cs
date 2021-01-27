using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "ID")]
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        

        [Display(Name = "Status")]
        public ProductStatus ProductStatus { get; set; }

        [Display(Name = "Product Category ID")]
        public int ProductCategoryID { get; set; }
        [ForeignKey("ProductCategoryID")]
        public virtual ProductCategory ProductCategory { get; set; }


        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }

        [Display(Name = "Unit Of Measurement ID")]
        public int ProductUnitOfMeasurementID { get; set; }
        [ForeignKey("ProductUnitOfMeasurementID")]
        public virtual ProductUnitOfMeasurement ProductUnitOfMeasurement { get; set; }

        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }
    }
}