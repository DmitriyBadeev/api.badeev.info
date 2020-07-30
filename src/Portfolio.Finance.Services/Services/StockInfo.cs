using System;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class StockInfo : IAssetInfo
    {
        private readonly string _ticket;
        private readonly StockResponse _data;
        private readonly int _amount;
        private readonly int _boughtPrice;
        private readonly DateTime _boughtDate;

        public StockInfo(StockMarketData marketData, string ticket, int amount, int boughtPrice, DateTime boughtDate)
        {
            _ticket = ticket;
            _amount = amount;
            _boughtPrice = boughtPrice;
            _boughtDate = boughtDate;
            _data = marketData.GetStockData(ticket).Result;
        }

        public double GetPrice()
        {
            var strPrice = FinanceHelpers.GetValueOfColumn("LAST", _data);

            if (strPrice == null)
            {
                throw new NullReferenceException("Price has not found");
            }

            var price = double.Parse(strPrice);
            return price;
        }

        public double GetPaperProfit()
        {
            var price = GetPrice();
            
            var boughtPrice = FinanceHelpers.GetPriceDouble(_boughtPrice);
            var allPrice = price * _amount;
            return allPrice - boughtPrice;
        }
    }
}
