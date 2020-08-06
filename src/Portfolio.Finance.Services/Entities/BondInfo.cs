using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Entities
{
    public class BondInfo : AssetInfo
    {
        private List<PaymentData> _paymentsData;
        private AssetResponse _data;

        public BondInfo(IStockMarketData marketData, string ticket) : base(marketData, ticket)
        {
        }

        public DateTime AmortizationDate
        {
            get
            {
                var paymentData = GetCouponsData().Result;
                return paymentData[paymentData.Count - 1].RegistryCloseDate;
            }
        }

        public bool HasAmortized => DateTime.Compare(AmortizationDate, DateTime.Now) <= 0;

        public override async Task<int> GetAllPrice()
        {
            var price = await GetPrice();
            
            if (price == -1)
            {
                return 0;
            }

            var nkd = FinanceHelpers.GetValueOfColumnSecurities("ACCRUEDINT", _data).GetDouble();
            var nominal = FinanceHelpers.GetValueOfColumnSecurities("FACEVALUE", _data).GetDouble();
            
            return FinanceHelpers.GetPriceInt((FinanceHelpers.GetPriceDouble(price) / 100 * nominal + nkd) * Amount);
        }

        public override List<PaymentData> PaymentsData
        {
            get => GetCouponsData().Result;
            protected set => _paymentsData = value;
        }

        private List<PaymentData> GetPaymentData(CouponsResponse responseData)
        {
            var paymentData = new List<PaymentData>();

            var dateIndex = responseData.coupons.columns.IndexOf("coupondate");
            var valueIndex = responseData.coupons.columns.IndexOf("value_rub");

            foreach (var couponJsonData in responseData.coupons.data)
            {
                var data = new PaymentData()
                {
                    Ticket = Ticket,
                    CurrencyId = "RUB",
                    PaymentValue = FinanceHelpers.GetPriceInt(couponJsonData[valueIndex].GetDouble()),
                    RegistryCloseDate = DateTime.ParseExact(couponJsonData[dateIndex].GetString(),
                        "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None)
                };

                paymentData.Add(data);
            }

            var amortizationDateIndex = responseData.amortizations.columns.IndexOf("amortdate");
            var amortizationDateString = responseData.amortizations.data[0][amortizationDateIndex].GetString();
            var amortizationValueIndex = responseData.amortizations.columns.IndexOf("value_rub");
            var amortizationData = responseData.amortizations.data[0];

            if (responseData.amortizations.data.Count != 0)
            {
                var data = new PaymentData()
                {
                    Ticket = Ticket,
                    CurrencyId = "RUB",
                    PaymentValue = FinanceHelpers.GetPriceInt(amortizationData[amortizationValueIndex].GetDouble()),
                    RegistryCloseDate = DateTime.ParseExact(amortizationDateString, "yyyy-MM-dd", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None)
                };

                paymentData.Add(data);
            }

            return paymentData;
        }

        protected override async Task<AssetResponse> GetData()
        {
            return _data ?? (_data = await _marketData.GetBondData(Ticket));
        }

        private async Task<List<PaymentData>> GetCouponsData()
        {
            if (_paymentsData == null)
            {
                var firstDate = GetMostEarlyOperationDate();
                var dividendsResponse = await _marketData.GetCouponsData(Ticket, firstDate);
                PaymentsData = GetPaymentData(dividendsResponse);
            }

            return _paymentsData;
        }

        private DateTime GetMostEarlyOperationDate()
        {
            return _operations.Aggregate(DateTime.MaxValue, (mostEarly, operation) =>
            {
                if (DateTime.Compare(operation.Date, mostEarly) <= 0)
                {
                    return operation.Date;
                }

                return mostEarly;
            });
        }
    }
}
