using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class ProductCategory
    {
        [Key]
        [Display(Name = "ID")]
        public int ProductCategoryID { get; set; }
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        
        [Display(Name = "VAT Category ID")]
        public int VatCategoryID { get; set; }
        [ForeignKey("VatCategoryID")]
        public virtual VatCategory VatCategory { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}