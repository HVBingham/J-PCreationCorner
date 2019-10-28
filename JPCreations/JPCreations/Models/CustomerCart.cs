using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class CustomerCart
    {
        public List<Product> Products { get; set; }

        [DataType(DataType.Currency)]
        public double Total { get; set; }
    }
}