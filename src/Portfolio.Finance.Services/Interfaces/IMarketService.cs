using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Services;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IMarketService
    {
        IEnumerable<AssetOperation> GetAllAssetOperations(int portfolioId);
        int GetAllPaperPrice(int userId); 
        int GetAllPaperProfit(int userId);
        int GetAllCost(int userId);
        int GetAllPaymentProfit(int userId);
        Task<OperationResult> BuyAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date);
        Task<OperationResult> SellAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date);
        IEnumerable<StockInfo> GetStocks(int portfolioId, int userId);
    }
}