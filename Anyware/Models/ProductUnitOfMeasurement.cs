using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Anyware.Models
{
    public class ProductUnitOfMeasurement
    {
        [Display(Name = "ID")]
        public int ProductUnitOfMeasurementID { get; set; }
        [Display(Name = "Unit of Measurement")]
        public string UnitName { get; set; }
        [Display(Name = "Abbreviation")]
        public string UnitAbbreviation { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}