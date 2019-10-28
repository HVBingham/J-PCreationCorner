using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class ProductsIndexView
    {
        public List<Product> Products { get; set; }
        public Customer Customer { get; set; }
    }
}