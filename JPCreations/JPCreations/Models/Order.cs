using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JPCreations.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Display(Name ="Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name ="Ordered Items")]
        public ICollection<Item> Items { get; set; }
        [Display(Name ="Sub Total Before Shipping")]
        public double OrderAmount { get; set; }
        [Display(Name ="Shipping")]
        public double ShippingPrice { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        [Display(Name ="Customer Details")]
        public Customer Customer { get; set; }

    }
}