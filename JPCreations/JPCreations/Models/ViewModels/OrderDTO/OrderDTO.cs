using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JPCreations.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JPCreations.ViewModels.OrderDTO
{
    [Table("tblOrders")]
    public class OrderDTO
    {
        [Key]
        public int OrderId { get; set; }
        
        public DateTime OrderDate { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

    }

}