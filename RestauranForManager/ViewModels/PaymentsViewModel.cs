using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestauranForManager.ViewModels
{
    public class PaymentsViewModel
    {
        public int NumberPaidByCard { get; set; }
        public int NumberPaidByWallet { get; set; }
        public double AmountMoneyPaidCard { get; set; }
        public double AmountMoneyPaidByWallet { get; set; }
        public double TipsAverage { get; set; }
        public double TipsNumbers { get; set; }
        public double TipsSum { get; set; }
    }
}
