using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Entities
{
    public abstract class AssetInfo
    {
        protected readonly IStockMarketData _marketData;
        protected readonly List<AssetOperation> _operations;

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

            if (jsonPrice.ValueKind == JsonValueKind.Undefined)
            {
                return -1;
            }

            var price = jsonPrice.GetDouble();

            return FinanceHelpers.GetPriceInt(price);
        }

        public abstract Task<int> GetAllPrice();

        public async Task<int> GetPaperProfit()
        {
            var allPrice = await GetAllPrice();
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

        public List<PaymentData> GetPaidPayments()
        {
            var paidPayments = new List<PaymentData>();

            foreach (var paymentData in PaymentsData)
            {
                var assetInfoAtPaymentDay = GetAssetInfoAt(paymentData.RegistryCloseDate);

                if (assetInfoAtPaymentDay != null && assetInfoAtPaymentDay.Amount > 0)
                {
                    var payment = new PaymentData()
                    {
                        PaymentValue = paymentData.PaymentValue * assetInfoAtPaymentDay.Amount,
                        RegistryCloseDate = paymentData.RegistryCloseDate,
                        CurrencyId = paymentData.CurrencyId,
                        Ticket = paymentData.Ticket
                    };

                    paidPayments.Add(payment);
                }
            }

            return paidPayments;
        }

        public List<PaymentData> GetFuturePayment()
        {
            return PaymentsData.FindAll(d => DateTime.Compare(DateTime.Now, d.RegistryCloseDate) <= 0);
        }

        public int GetSumPayments()
        {
            return GetPaidPayments().Aggregate(0, (total, payment) => total + payment.PaymentValue);
        }

        private AssetInfo GetAssetInfoAt(DateTime date)
        {
            if (DateTime.Compare(DateTime.Now, date) <= 0)
            {
                return null;
            }

            var assetInfo = new StockInfo(_marketData, Ticket);

            var operations = _operations.FindAll(o => DateTime.Compare(o.Date, date) <= 0);

            foreach (var operation in operations)
            {
                assetInfo.RegisterOperation(operation);
            }

            return assetInfo;
        }

        public abstract List<PaymentData> PaymentsData { get; protected set; }

        protected abstract Task<AssetResponse> GetData();
    }
}