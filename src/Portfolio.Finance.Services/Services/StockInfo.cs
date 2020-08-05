using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class StockInfo : IAssetInfo
    {
        private readonly List<AssetOperation> _operations;
        private readonly IStockMarketData _marketData;
        private List<PaymentData> _paymentsData;
        private AssetResponse _data;

        public StockInfo(IStockMarketData marketData, string ticket)
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
        public List<PaymentData> PaymentsData
        {
            get
            {
                return GetDividendData().Result;
            }
            private set => _paymentsData = value;
        }

        public async Task<string> GetName()
        {
            var data = await GetData();

            var indexName = data.securities.columns.IndexOf("SHORTNAME");
            return data.securities.data[0][indexName].GetString();
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

        public List<PaymentData> GetFuturePayment()
        {
            return PaymentsData.FindAll(d => DateTime.Compare(DateTime.Now, d.RegistryCloseDate) <= 0);
        }

        public int GetSumPayments()
        {
            return GetPaidPayments().Aggregate(0, (total, payment) => total + payment.PaymentValue);
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

        private IAssetInfo GetAssetInfoAt(DateTime date)
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

        private List<PaymentData> GetDividendsData(DividendsResponse responseData)
        {
            var dividendsData = new List<PaymentData>();

            var ticketIndex = responseData.dividends.columns.IndexOf("secid");
            var dateIndex = responseData.dividends.columns.IndexOf("registryclosedate");
            var valueIndex = responseData.dividends.columns.IndexOf("value");
            var currencyIndex = responseData.dividends.columns.IndexOf("currencyid");

            foreach (var dividendJsonData in responseData.dividends.data)
            {
                var data = new PaymentData()
                {
                    Ticket = dividendJsonData[ticketIndex].GetString(),
                    CurrencyId = dividendJsonData[currencyIndex].GetString(),
                    PaymentValue = (int)(dividendJsonData[valueIndex].GetDouble() * 100),
                    RegistryCloseDate = DateTime.ParseExact(dividendJsonData[dateIndex].GetString(), 
                        "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None)
                };

                dividendsData.Add(data);
            }

            return dividendsData;
        }

        private async Task<AssetResponse> GetData()
        {
            if (_data == null)
            {
                _data = await _marketData.GetStockData(Ticket);
            }

            return _data;
        }

        private async Task<List<PaymentData>> GetDividendData()
        {
            if (_paymentsData == null)
            {
                var dividendsResponse = await _marketData.GetDividendsData(Ticket);
                PaymentsData = GetDividendsData(dividendsResponse);
            }

            return _paymentsData;
        }
    }
}
