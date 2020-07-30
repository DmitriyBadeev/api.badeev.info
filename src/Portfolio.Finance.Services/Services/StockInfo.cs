using System.Collections.Generic;
using System.Text.Json;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class StockInfo : IAssetInfo
    {
        private readonly StockResponse _data;
        private readonly List<JsonElement> _stockInfoList;

        public StockInfo(IStockMarketData marketData, string ticket, int amount, int boughtPrice)
        {
            Ticket = ticket;
            Amount = amount;
            BoughtPrice = boughtPrice;
            _data = marketData.GetStockData(ticket).Result;
            _stockInfoList = FinanceHelpers.GetStockInfo("TQBR", _data);
        }

        public string Ticket { get; }
        public int Amount { get; private set; }
        public int BoughtPrice { get; private set; }

        public void RegisterOperation(AssetOperation operation)
        {
            if (operation.AssetAction.Name == SeedFinanceData.BUY_ACTION)
            {
                Amount += operation.Amount;
                BoughtPrice += operation.PaymentPrice;
            }

            if (operation.AssetAction.Name == SeedFinanceData.SELL_ACTION)
            {
                Amount -= operation.Amount;
                BoughtPrice -= operation.PaymentPrice;
            }
        }

        public double GetPrice()
        {
            var jsonPrice = FinanceHelpers.GetValueOfColumn("LAST", _stockInfoList, _data);

            var price = jsonPrice.GetDouble();
            return price;
        }

        public double GetPaperProfit()
        {
            var price = GetPrice();
            
            var boughtPrice = FinanceHelpers.GetPriceDouble(BoughtPrice);
            var allPrice = price * Amount;
            return allPrice - boughtPrice;
        }
    }
}
