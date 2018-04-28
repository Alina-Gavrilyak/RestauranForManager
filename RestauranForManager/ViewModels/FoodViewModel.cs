using RestauranForManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranForManager.ViewModels
{
    public class FoodViewModel
    {
        public List<FoodViewModel> TopOrders { get; set; }
        public List<FoodViewModel> Unordered { get; set; }
        public OrderItems OrderItems { get; set; }
        public int NumberOreders { get; set; }
        public double AmountPrice { get; set; }

        public FoodViewModel()
        {
            TopOrders = new List<FoodViewModel>();
            Unordered = new List<FoodViewModel>();
        }
    }
}
