using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class ProductsViewModel
    {
        public List<Product> products;
        public ProductsViewModel()
        {
            this.products = new List<Product>();
        }
        public List<Product> findAll()
        {
            return this.products;
        }

        public Product find(int id)
        {
            return this.products.FirstOrDefault(p => p.Id.Equals(id));
        }
        public Customer Customer { get; set; }

    }
}