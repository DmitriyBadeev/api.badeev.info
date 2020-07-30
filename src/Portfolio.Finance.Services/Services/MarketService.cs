using System.Collections.Generic;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class MarketService : IMarketService
    {
        private List<IAssetInfo> _assets;

        public MarketService(IAssetsFactory assetsFactory)
        {
            _assets = assetsFactory.Create();
        }
    }
}
