using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranForManager.ViewModels
{
    public class OrdersViewModel
    {
        public double AverageCheck { get; set; }
        public double AmountMoney { get; set; }
        public int NumberOrders { get; set; }
        public int NumberItemsOrdered { get; set; }

        public OrdersViewModel()
        {

        }
    }
}
