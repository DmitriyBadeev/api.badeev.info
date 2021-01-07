using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class AssetsFactory : IAssetsFactory
    {
        private readonly FinanceDataService _financeDataService;
        private readonly IStockMarketData _marketData;

        public AssetsFactory(FinanceDataService financeDataService, IStockMarketData marketData)
        {
            _financeDataService = financeDataService;
            _marketData = marketData;
        }

        public List<AssetInfo> Create(int portfolioId)
        {
            var assets = new List<AssetInfo>();

            var operations = _financeDataService.EfContext.AssetOperations
                .Where(o => o.PortfolioId == portfolioId)
                .Include(o => o.AssetAction)
                .Include(o => o.AssetType);

            foreach (var assetOperation in operations)
            {
                RegisterOperation(assets, assetOperation);
            }

            return assets.FindAll(a => a.Amount != 0);
        }

        private void RegisterOperation(List<AssetInfo> assets, AssetOperation operation)
        {
            if (operation.AssetType.Name == SeedFinanceData.STOCK_ASSET_TYPE)
            {
                var asset = assets.FirstOrDefault(a => a.Ticket == operation.Ticket);

                if (asset == null)
                {
                    var stockInfo = new StockInfo(_marketData, _financeDataService, operation.Ticket);
                    assets.Add(stockInfo);
                    asset = stockInfo;
                }
                
                asset.RegisterOperation(operation);
            }

            if (operation.AssetType.Name == SeedFinanceData.FOND_ASSET_TYPE)
            {
                var asset = assets.FirstOrDefault(a => a.Ticket == operation.Ticket);

                if (asset == null)
                {
                    var fondInfo = new FondInfo(_marketData, _financeDataService, operation.Ticket);
                    assets.Add(fondInfo);
                    asset = fondInfo;
                }

                asset.RegisterOperation(operation);
            }

            if (operation.AssetType.Name == SeedFinanceData.BOND_ASSET_TYPE)
            {
                var asset = assets.FirstOrDefault(a => a.Ticket == operation.Ticket);

                if (asset == null)
                {
                    var bondInfo = new BondInfo(_marketData, _financeDataService, operation.Ticket);
                    assets.Add(bondInfo);
                    asset = bondInfo;
                }

                asset.RegisterOperation(operation);
            }
        }
    }
}
