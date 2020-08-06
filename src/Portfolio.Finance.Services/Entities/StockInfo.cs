using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Entities
{
    public class StockInfo : AssetInfo
    {
        private List<PaymentData> _paymentsData;
        private AssetResponse _data;

        public StockInfo(IStockMarketData marketData, string ticket) : base(marketData, ticket)
        {
        }

        public override List<PaymentData> PaymentsData
        {
            get => GetDividendData().Result;
            protected set => _paymentsData = value;
        }

        public override async Task<int> GetAllPrice()
        {
            var price = await GetPrice();
            return price * Amount;
        }

        private List<PaymentData> GetPaymentData(DividendsResponse responseData)
        {
            var paymentData = new List<PaymentData>();

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

                paymentData.Add(data);
            }

            return paymentData;
        }

        protected override async Task<AssetResponse> GetData()
        {
            return _data ?? (_data = await _marketData.GetStockData(Ticket));
        }

        private async Task<List<PaymentData>> GetDividendData()
        {
            if (_paymentsData == null)
            {
                var dividendsResponse = await _marketData.GetDividendsData(Ticket);
                PaymentsData = GetPaymentData(dividendsResponse);
            }

            return _paymentsData;
        }
    }
}
