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
        [DataType(DataType.Currency)]
        public double Price { get; set; } 
        [Display(Name ="Currently An Active item?")]
        public bool IsActive { get; set; }
        public int Quantity { get; set; }
        public int PurchaseQuantity { get; set; }
        [ForeignKey("Image")]
        [Display(Name ="Product Image")]
        public int? ImageId { get; set; }
        public Image Image { get; set; }
        public IEnumerable<Image> Images { get; set; }

      
        
        
    }
}