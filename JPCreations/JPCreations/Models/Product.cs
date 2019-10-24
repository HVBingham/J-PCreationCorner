using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="Product Name")]
        public string ProductName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Price { get; set; } 
        public ICollection<Image> Images { get; set; }
        
        
    }
}