using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JPCreations.ViewModels.OrderDTO;

namespace JPCreations.Models.ViewModels
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

        }
        public OrderViewModel(OrderDTO row )
        {
            OrderId = row.OrderId;
            CustomerId = row.CustomerId;
            OrderDate = row.OrderDate;
            Name = row.Customer.FirstName + " " + row.Customer.LastName;
        }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public double  Total { get; set; }
        public Dictionary<string, int> ProductsAndQty { get; set; }
        public DateTime OrderDate { get; set; }

    }
}