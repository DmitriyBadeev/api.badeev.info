using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public abstract class AssetInfo
    {
        private readonly IStockMarketData _marketData;
        private readonly List<AssetOperation> _operations;

        protected AssetInfo(IStockMarketData marketData, string ticket)
        {
            Ticket = ticket;
            Amount = 0;
            BoughtPrice = 0;
            _marketData = marketData;
            _operations = new List<AssetOperation>();
        }

        public string Ticket { get; }
        public int Amount { get; private set; }
        public int BoughtPrice { get; private set; }

        public async Task<string> GetName()
        {
            var data = await GetData();

            return FinanceHelpers.GetValueOfColumnSecurities("SHORTNAME", data).GetString();
        }

        public async Task<int> GetPrice()
        {
            var data = await GetData();

            var jsonPrice = FinanceHelpers.GetValueOfColumnMarketdata("LAST", data);

            var price = jsonPrice.GetDouble() * 100;
            return (int)price;
        }

        public async Task<int> GetPaperProfit()
        {
            var price = await GetPrice();

            var allPrice = price * Amount;
            return allPrice - BoughtPrice;
        }

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

            _operations.Add(operation);
        }

        public abstract List<PaymentData> PaymentsData { get; }

        public abstract List<PaymentData> GetFuturePayment();

        public abstract List<PaymentData> GetPaidPayments();

        public abstract int GetSumPayments();

        protected abstract Task<AssetResponse> GetData();
    }
}