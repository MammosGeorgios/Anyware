using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "ID")]
        public int OrderID { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }    


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime DateStarted { get; set; } 


        public OrderStatus OrderStatus { get; set; }


        public ICollection<ProductInOrder> ProductInOrder { get; set; }
    }
}