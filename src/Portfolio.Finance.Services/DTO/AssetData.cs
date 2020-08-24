using System.Collections.Generic;

namespace Portfolio.Finance.Services.DTO
{
    public class AssetData
    {
        public string Name { get; set; }

        public string Ticket { get; set; }

        public int Amount { get; set; }

        public double Price { get; set; }

        public double PriceChange { get; set; }

        public double AllPrice { get; set; }

        public double BoughtPrice { get; set; }

        public double PaperProfit { get; set; }

        public double PaperProfitPercent { get; set; }

        public PaymentData NearestDividend { get; set; }

        public IEnumerable<PaymentData> Payments { get; set; }

        public double PaidDividends { get; set; }

        public string UpdateTime { get; set; }
    }
}
