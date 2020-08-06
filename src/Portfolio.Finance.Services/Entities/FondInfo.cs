using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Entities
{
    public class FondInfo : AssetInfo
    {
        private AssetResponse _data;

        public FondInfo(IStockMarketData marketData, string ticket) : base(marketData, ticket)
        {
            PaymentsData = new List<PaymentData>();
        }

        public sealed override List<PaymentData> PaymentsData { get; protected set; }

        public override async Task<int> GetAllPrice()
        {
            var price = await GetPrice();
            return price * Amount;
        }

        protected override async Task<AssetResponse> GetData()
        {
            return _data ?? (_data = await _marketData.GetFondData(Ticket));
        }
    }
}
