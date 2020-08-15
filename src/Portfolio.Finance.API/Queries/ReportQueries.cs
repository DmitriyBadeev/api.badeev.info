using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.API.Queries.Response;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class ReportQueries
    {
        [Authorize]
        public AllPortfoliosReport GetAllPortfoliosReport([CurrentUserIdGlobalState] int userId, 
            [Service] IMarketService marketService, [Service] IBalanceService balanceService)
        {
            return QueryGetters.GetAllPortfoliosReport(userId, marketService, balanceService);
        }

        [Authorize]
        public async Task<List<StockReport>> GetStockReports([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService, int portfolioId)
        {
            return await QueryGetters.GetStockReports(userId, marketService, portfolioId);
        }

        [Authorize]
        public async Task<List<FondReport>> GetFondReports([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService, int portfolioId)
        {
            return await QueryGetters.GetFondReports(userId, marketService, portfolioId);
        }

        [Authorize]
        public async Task<List<BondReport>> GetBondReports([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService, int portfolioId)
        {
            return await QueryGetters.GetBondReports(userId, marketService, portfolioId);
        }

        [Authorize]
        public async Task<AssetPrices> GetAllAssetPricesReport([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService)
        {
            return await QueryGetters.GetAllAssetPricesReport(userId, marketService);
        }
    }
}