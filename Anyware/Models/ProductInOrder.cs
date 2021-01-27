using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class ProductInOrder
    {
        [Key]
        [Display(Name = "ID")]
        public int ProductInOrderID { get; set; }

        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
       

        [Display(Name = "Order ID")]
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        public int Quantity { get; set; }
    }
}