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
        List<PaymentData> GetAllFuturePayments(int userId);
        Task<AssetPrices> GetAllAssetPrices(int userId);
        Task<OperationResult> BuyAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date);
        Task<OperationResult> SellAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date);
    }
}