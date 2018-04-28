﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranForManager.Models
{
    public class OrderItemsOrder
    {
        public int Id { get; set; }
        public Order Order { get; set; }
        public OrderItems OrderItems { get; set; }
        public double PriceDishNow { get; set; }
    }
}
