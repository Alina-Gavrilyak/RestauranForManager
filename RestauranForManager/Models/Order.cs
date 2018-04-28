using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranForManager.Models
{
    public class Order
    {
        public int Id { get; set; }
        public virtual ICollection<OrderItemsOrder> Items { get; set; }
        public Order()
        {
            Items = new List<OrderItemsOrder>();
           
        }
        public double Tips { get; set;}
        public double TotalPrice { get; set; }
        public DateTime OpenedDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public State State { get; set; }
        public bool CheckAmount { get; set; }
        public CheckPaidBy CheckPaidBy { get; set; }
    }
    public enum CheckPaidBy
    {
        Card,
        Wallet
    }
}
