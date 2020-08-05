using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class FondInfo : IAssetInfo
    {
        private readonly IStockMarketData _marketData;
        private AssetResponse _data;

        public FondInfo(IStockMarketData marketData, string ticket)
        {
            _marketData = marketData;
            Ticket = ticket;
            Amount = 0;
            BoughtPrice = 0;
            PaymentsData = new List<PaymentData>();
        }

        public string Ticket { get; }
        public int Amount { get; private set; }
        public int BoughtPrice { get; private set; }
        public List<PaymentData> PaymentsData { get; }

        public async Task<string> GetName()
        {
            var data = await GetData();

            return FinanceHelpers.GetValueOfColumnSecurities("SHORTNAME", data).GetString();
        }

        public List<PaymentData> GetFuturePayment()
        {
            return PaymentsData;
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

        public List<PaymentData> GetPaidPayments()
        {
            return PaymentsData;
        }

        public int GetSumPayments()
        {
            return 0;
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
        }

        private async Task<AssetResponse> GetData()
        {
            if (_data == null)
            {
                _data = await _marketData.GetFondData(Ticket);
            }

            return _data;
        }
    }
}
