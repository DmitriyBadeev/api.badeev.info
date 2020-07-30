using System.Collections.Generic;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class MarketService : IMarketService
    {
        private readonly IDataService _financeDataService;
        private List<IAssetInfo> _assets;

        public MarketService(IDataService financeDataService)
        {
            _financeDataService = financeDataService;
            _assets = new List<IAssetInfo>();
        }
    }
}
