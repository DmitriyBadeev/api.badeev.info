using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class AssetsFactory : IAssetsFactory
    {
        private readonly FinanceDataService _financeDataService;
        private readonly IStockMarketData _marketData;
        private readonly List<IAssetInfo> _assets;

        public AssetsFactory(FinanceDataService financeDataService, IStockMarketData marketData)
        {
            _financeDataService = financeDataService;
            _marketData = marketData;
            _assets = new List<IAssetInfo>();
        }

        public List<IAssetInfo> Create()
        {
            var operations = _financeDataService.EfContext.AssetOperations
                .Include(o => o.AssetAction)
                .Include(o => o.AssetType);

            foreach (var assetOperation in operations)
            {
                RegisterOperation(assetOperation);
            }

            return _assets;
        }

        private void RegisterOperation(AssetOperation operation)
        {
            if (operation.AssetType.Name == SeedFinanceData.STOCK_ASSET_TYPE)
            {
                var asset = _assets.FirstOrDefault(a => a.Ticket == operation.Ticket);

                if (asset == null)
                {
                    var stockInfo = new StockInfo(_marketData, operation.Ticket, operation.Amount, operation.PaymentPrice);
                    _assets.Add(stockInfo);
                    return;
                }
                
                asset.RegisterOperation(operation);
            }

            if (operation.AssetType.Name == SeedFinanceData.FOND_ASSET_TYPE)
            {
                throw new NotImplementedException();
            }

            if (operation.AssetType.Name == SeedFinanceData.BOND_ASSET_TYPE)
            {
                throw new NotImplementedException();
            }
        }
    }
}
