using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranForManager.Models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set;}
        public string Media { get; set; }
        public virtual ICollection<OrderItemsOrder> Orders { get; set; }
        public OrderItems()
        {
            Orders = new List<OrderItemsOrder>();
        }
    }
}
