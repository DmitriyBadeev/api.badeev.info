using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Portfolio.Finance.API.Queries;
using Portfolio.Finance.API.Queries.Response;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Subscriptions
{
    [ExtendObjectType(Name = "Subscriptions")]
    public class ReportSubscriptions
    {
        [Subscribe]
        [Topic]
        public AllPortfoliosReport OnUpdatePortfoliosReport(
            [Service] IMarketService marketService,
            [Service] IBalanceService balanceService,
            [EventMessage] int userId)
        {
            return QueryGetters.GetAllPortfoliosReport(userId, marketService, balanceService);
        }

        [Subscribe]
        [Topic]
        public async Task<List<StockReport>> OnUpdateStockReports(
            [Service] IMarketService marketService,
            [EventMessage] int userId, int portfolioId)
        {
            return await QueryGetters.GetStockReports(userId, marketService, portfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<List<FondReport>> OnUpdateFondReports(
            [Service] IMarketService marketService,
            [EventMessage] int userId, int portfolioId)
        {
            return await QueryGetters.GetFondReports(userId, marketService, portfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<List<BondReport>> OnUpdateBondReports(
            [Service] IMarketService marketService,
            [EventMessage] int userId, int portfolioId)
        {
            return await QueryGetters.GetBondReports(userId, marketService, portfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<AssetPrices> OnUpdatePricesReport(
            [Service] IMarketService marketService,
            [EventMessage] int userId)
        {
            return await QueryGetters.GetAllAssetPricesReport(userId, marketService);
        }
    }
}
