using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class VatCategory
    {
        [Display(Name = "ID")]
        public int VatCategoryID { get; set; }

        [Display(Name = "VAT")]
        public string VatName { get; set; }

        [Display(Name = "Percentage")]
        public double VatPercentage { get; set; }
    }
}