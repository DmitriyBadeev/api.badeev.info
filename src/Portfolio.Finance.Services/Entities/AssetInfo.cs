using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.DTO.Responses;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Entities
{
    public abstract class AssetInfo
    {
        protected readonly IStockMarketData MarketData;
        protected readonly List<AssetOperation> Operations;
        protected AssetResponse Data;
        
        private readonly FinanceDataService _financeDataService;

        protected AssetInfo(IStockMarketData marketData, FinanceDataService financeDataService, string ticket)
        {
            Ticket = ticket;
            Amount = 0;
            BoughtPrice = 0;
            MarketData = marketData;
            _financeDataService = financeDataService;
            Operations = new List<AssetOperation>();
        }

        public string Ticket { get; }
        public int Amount { get; private set; }
        public int BoughtPrice { get; private set; }

        public async Task<string> GetName()
        {
            var data = await GetData();
            var result = FinanceHelpers.GetValueOfColumnSecurities("SHORTNAME", data);
            try
            {
                return result.GetString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public async Task<double> GetPrice()
        {
            var data = await GetData();

            var jsonPrice = FinanceHelpers.GetValueOfColumnMarketdata("LAST", data);
            var jsonDecimals = FinanceHelpers.GetValueOfColumnSecurities("DECIMALS", data);

            if (jsonPrice.ValueKind == JsonValueKind.Undefined)
            {
                return -1;
            }

            var price = jsonPrice.GetDouble();
            var decimals = jsonDecimals.GetInt32();

            return Math.Round(price, decimals);
        }

        public async Task<int> GetPriceChange()
        {
            var data = await GetData();
            var jsonPriceChange = FinanceHelpers.GetValueOfColumnMarketdata("LASTTOPREVPRICE", data);

            if (jsonPriceChange.ValueKind == JsonValueKind.Undefined)
            {
                return -1;
            }

            var changePercent = jsonPriceChange.GetDouble();
            return FinanceHelpers.GetPriceInt(changePercent);
        }

        public abstract Task<int> GetAllPrice();

        public async Task<int> GetPaperProfit()
        {
            var allPrice = await GetAllPrice();

            return allPrice - BoughtPrice;
        }

        public async Task<double> GetPaperProfitPercent()
        {
            var profit = await GetPaperProfit();
            return FinanceHelpers.DivWithOneDigitRound(profit, BoughtPrice);
        }

        public async Task<string> GetUpdateTime()
        {
            var data = await GetData();
            var jsonUpdateTime = FinanceHelpers.GetValueOfColumnMarketdata("TIME", data);

            if (jsonUpdateTime.ValueKind == JsonValueKind.Undefined)
            {
                return "";
            }

            var updateTime = jsonUpdateTime.GetString();
            return updateTime;
        }

        public PaymentData GetNearestPayment()
        {
            var futurePayments = GetFuturePayments();
            futurePayments.Sort((p1, p2) => DateTime.Compare(p1.RegistryCloseDate, p2.RegistryCloseDate));

            if (futurePayments.Count > 0)
            {
                return futurePayments[0];
            }

            return null;
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

            Operations.Add(operation);
        }

        public List<Payment> GetPaidPayments()
        {
            return _financeDataService.EfContext.Payments
                .Where(p => p.Ticket == Ticket)
                .ToList();
            
            // var paidPayments = new List<PaymentData>();
            //
            // foreach (var paymentData in PaymentsData)
            // {
            //     var assetInfoAtPaymentDay = GetAssetInfoAt(paymentData.RegistryCloseDate);
            //
            //     if (assetInfoAtPaymentDay != null && assetInfoAtPaymentDay.Amount > 0)
            //     {
            //         var payment = new PaymentData()
            //         {
            //             Name = GetName().Result,
            //             Ticket = paymentData.Ticket,
            //             Amount = assetInfoAtPaymentDay.Amount,
            //             PaymentValue = paymentData.PaymentValue,
            //             AllPayment = paymentData.PaymentValue * assetInfoAtPaymentDay.Amount,
            //             RegistryCloseDate = paymentData.RegistryCloseDate,
            //             CurrencyId = paymentData.CurrencyId,
            //         };
            //
            //         paidPayments.Add(payment);
            //     }
            // }
            //
            // return paidPayments;
        }

        public List<PaymentData> GetFuturePayments()
        {
            return PaymentsData.FindAll(d => DateTime.Compare(DateTime.Now, d.RegistryCloseDate) <= 0);
        }

        public int GetSumPayments()
        {
            return GetPaidPayments().Aggregate(0, (total, payment) => total + payment.PaymentValue);
        }

        // private AssetInfo GetAssetInfoAt(DateTime date)
        // {
        //     if (DateTime.Compare(DateTime.Now, date) <= 0)
        //     {
        //         return null;
        //     }
        //
        //     var assetInfo = new StockInfo(MarketData, _financeDataService, Ticket);
        //
        //     var operations = Operations.FindAll(o => DateTime.Compare(o.Date, date) <= 0);
        //
        //     foreach (var operation in operations)
        //     {
        //         assetInfo.RegisterOperation(operation);
        //     }
        //
        //     return assetInfo;
        // }

        public abstract List<PaymentData> PaymentsData { get; protected set; }

        protected abstract Task<AssetResponse> GetData();
    }
}