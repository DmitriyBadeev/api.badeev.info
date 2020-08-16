using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IMarketService
    {
        IEnumerable<AssetOperation> GetAllAssetOperations(int portfolioId);
        int GetAllPaperPrice(int userId);
        double GetPercentOfPaperProfit(int userId);
        double GetPercentOfPaymentProfit(int userId);
        int GetAllPaperProfit(int userId);
        int GetAllCost(int userId);
        int GetAllPaymentProfit(int userId);
        List<PaymentData> GetAllFuturePayments(int userId);
        Task<AssetPrices> GetAllAssetPrices(int userId);
        int GetUserBalanceWithPaidPayments(int userId);
        Task<OperationResult> BuyAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date);
        Task<OperationResult> SellAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date);
        IEnumerable<StockInfo> GetStocks(int userId, int portfolioId);
        IEnumerable<FondInfo> GetFonds(int userId, int portfolioId);
        IEnumerable<BondInfo> GetBonds(int userId, int portfolioId);
    }
}