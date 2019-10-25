using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Product { get; set; }
        public IEnumerable<Image> Image { get; set; }

    }
}