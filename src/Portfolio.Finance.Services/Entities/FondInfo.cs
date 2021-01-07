using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.DTO.Responses;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Entities
{
    public class FondInfo : AssetInfo
    {
        public FondInfo(IStockMarketData marketData, FinanceDataService financeDataService, string ticket) 
            : base(marketData, financeDataService, ticket)
        {
            PaymentsData = new List<PaymentData>();
        }

        public sealed override List<PaymentData> PaymentsData { get; protected set; }

        public override async Task<int> GetAllPrice()
        {
            var price = await GetPrice();

            return FinanceHelpers.GetPriceInt(price * Amount);
        }

        protected override async Task<AssetResponse> GetData()
        {
            return Data ?? (Data = await MarketData.GetFondData(Ticket));
        }
    }
}
