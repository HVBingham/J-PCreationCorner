using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class Item
    {
       [Key]

        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}