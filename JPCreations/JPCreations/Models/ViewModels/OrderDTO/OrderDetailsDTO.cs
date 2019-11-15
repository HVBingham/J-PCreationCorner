using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JPCreations.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JPCreations.ViewModels.OrderDTO
{
    [Table("tblOrderDetails")]
    public class OrderDetailsDTO
    {
        [Key]
        public int Id { get; set; }
              
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey("OrderDTO")]
         public int? OrderId { get; set; }
        public OrderDTO OrderDTO { get; set; }
        [ForeignKey("Products")]
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public ICollection<Product> Products { get; set; }
        public int Quantity { get; set; }
        
    }
}