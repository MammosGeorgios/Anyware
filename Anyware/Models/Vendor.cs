using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class Vendor
    {
        [Key]
        [Display(Name = "ID")]
        public int VendorID { get; set; }

        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }
       
        [Display(Name = "AFM")]
        [RegularExpression(@"^(\d{9})$", ErrorMessage = "Please enter a valid AFM")]
        public string VendorAFM { get; set; }

        [Display(Name = "Legal Name")]
        public string VendorLegalName { get; set; }

        [Display(Name = "DOI")]
        public string VendorDOI { get; set; }

        [Display(Name = "Secret Key")]
        public string VendorSecretKey { get; set; }
    }
}