using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestauranForManager.Data;
using RestauranForManager.Models;
using RestauranForManager.ViewModels;

namespace RestauranForManager.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db;
        private readonly int counColumn = 7;
        public HomeController (ApplicationDbContext context)
        {
            UpdateTotalPrice(context);
            db = context;
        }

        private static void UpdateTotalPrice(ApplicationDbContext context)
        {
            List<Order> orders = context.Orders.ToList();
            foreach (Order item in orders)
            {
                item.TotalPrice = context.OrderItemsOrder.Where(o => o.Order == item && (o.Order.State.Id == 3)).Sum(o => o.PriceDishNow);
            }
            context.SaveChanges();
        }

        public IActionResult Index()
        {
            OrdersViewModel ovm = GetOrdersViewModel();
            return View(ovm);
        }


        private OrdersViewModel GetOrdersViewModel()
        {
            double amountMoney = db.OrderItemsOrder.Sum(o => o.OrderItems.Price);
            int numberOrders = db.Orders.Count();
            double averageCheck = 0;
            if (numberOrders!=0)
             averageCheck = amountMoney / numberOrders;
            int numberItemsOrdered = db.OrderItemsOrder.Count();

            return new OrdersViewModel()
            {
                AverageCheck = averageCheck,
                AmountMoney = amountMoney,
                NumberItemsOrdered = numberItemsOrdered,
                NumberOrders = numberOrders
            };
        }

        [HttpPost]
        public IActionResult GetChartViewModel(NumType type, Period period)
        {
            ChartViewModel cvm;
            switch (period)
            {
                case Period.Day:
                    cvm = GetChartVMByDay(type);
                    break;
                case Period.Week:
                    cvm = GetChartVMByWeek(type);
                    break;
                case Period.Month:
                    cvm = GetChartVMByMonth(type);
                    break;
                default:
                    cvm = new ChartViewModel();
                    break;
            }
            return PartialView(cvm);
        }


        #region GetChartVM
        private ChartViewModel GetChartVMByDay(NumType type)
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = counColumn-1; i >= 0; i--)
            {
                DateTime date = now.AddDays(-i);
                IQueryable<Order> query = db.Orders.Where(o => o.ClosedDate.Date.Equals(date));
                double count = GetCountFromQuery(query, type);
                counts.Add(count);
            }

            return new ChartViewModel(counts, type, Period.Day);
        }

        private ChartViewModel GetChartVMByWeek(NumType type)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = counColumn-1; i >= 0; i--)
            {
                DateTime date = now.AddDays(-7 * i);
                int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - 1;
                IQueryable<Order> query = db.Orders
                    .Where(o =>
                        o.ClosedDate.Year.Equals(date.Year) &&
                        ((o.ClosedDate.DayOfYear - 1) / 7).Equals(week));
                double count = GetCountFromQuery(query, type);
                counts.Add(count);
            }

            return new ChartViewModel(counts, type, Period.Week);
        }

        private ChartViewModel GetChartVMByMonth(NumType type)
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = counColumn-1; i >= 0; i--)
            {
                DateTime date = now.AddMonths(-i);
                IQueryable<Order> query = db.Orders.Where(o =>
                    o.ClosedDate.Year.Equals(date.Year) &&
                    o.ClosedDate.Month.Equals(date.Month));
                double count = GetCountFromQuery(query, type);
                counts.Add(count);
            }

            return new ChartViewModel(counts, type, Period.Month);
        }

        private double GetCountFromQuery(IQueryable<Order> query, NumType type)
        {
            switch (type)
            {
                case NumType.Money:
                    return query.Sum(o => o.TotalPrice);
                case NumType.Orders:
                    return query.Count();
                default:
                    return 0;
            }
        }
        #endregion

        public IActionResult GetFoodViewModel()
        {
            List<FoodViewModel> topOrders = new List<FoodViewModel>();
            List<FoodViewModel> unordered = new List<FoodViewModel>();

            topOrders = db.OrderItemsOrder.GroupBy(o => o.OrderItems).OrderByDescending(o => o.Count()).Select(o => new FoodViewModel { OrderItems = o.Key, NumberOreders = o.Count(), AmountPrice = o.Key.Price * o.Count()}).Take(10).ToList();

            unordered = db.OrderItemsOrder.GroupBy(o => o.OrderItems).OrderBy(o=>o.Count()).Select(o=>new FoodViewModel { OrderItems = o.Key, NumberOreders=o.Count(), AmountPrice = o. Key.Price * o.Count()}).Take(10).ToList();


            FoodViewModel fvm = new FoodViewModel()
            {
                TopOrders = topOrders,
                Unordered = unordered
            };
            return PartialView(fvm);
        }

        public IActionResult GetPaymentsViewModel()
        {
            int numberPaidByCard = db.Orders.Count(o => o.CheckPaidBy == CheckPaidBy.Card);
            int numberPaidByWallet = db.Orders.Count(o => o.CheckPaidBy == CheckPaidBy.Wallet);

            double amountMoneyPaidCard = db.Orders.Where(o => o.CheckPaidBy == CheckPaidBy.Card).Sum(o => o.TotalPrice);
            double amountMoneyPaidByWallet = db.Orders.Where(o => o.CheckPaidBy == CheckPaidBy.Wallet).Sum(o => o.TotalPrice);
            double tipsAverage = 0;
            if (db.Orders.Sum(o => o.TotalPrice) !=0 )
                tipsAverage = db.Orders.Sum(o => o.Tips) / db.Orders.Sum(o => o.TotalPrice) * 100;
            int tipsNumbers = db.Orders.Count(o => o.Tips !=0.0);
            double tipsSum = db.Orders.Sum(o => o.Tips);

            PaymentsViewModel pvm = new PaymentsViewModel()
            {
                NumberPaidByCard = numberPaidByCard,
                NumberPaidByWallet = numberPaidByWallet,
                AmountMoneyPaidCard = amountMoneyPaidCard,
                AmountMoneyPaidByWallet = amountMoneyPaidByWallet,
                TipsAverage = tipsAverage,
                TipsNumbers = tipsNumbers,
                TipsSum = tipsSum
            };
            return PartialView(pvm);
        }
   
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
