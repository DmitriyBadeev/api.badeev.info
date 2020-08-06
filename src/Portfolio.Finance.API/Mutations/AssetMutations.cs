using System.Threading.Tasks;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.API.Mutations.InputTypes;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class AssetMutations
    {
        private readonly IMarketService _marketService;

        public AssetMutations(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [Authorize]
        public async Task<OperationResult> BuyAsset(BuyAssetInput buyAssetInput)
        {
            var result = await _marketService.BuyAsset(buyAssetInput.PortfolioId, buyAssetInput.Ticket, buyAssetInput.Price,
                buyAssetInput.Amount, buyAssetInput.AssetTypeId, buyAssetInput.Date);

            return result;
        }

        [Authorize]
        public async Task<OperationResult> SellAsset(SellAssetInput sellAssetInput)
        {
            var result = await _marketService.SellAsset(sellAssetInput.PortfolioId, sellAssetInput.Ticket, sellAssetInput.Price,
                sellAssetInput.Amount, sellAssetInput.AssetTypeId, sellAssetInput.Date);

            return result;
        }
    }
}