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
            [EventMessage] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService)
        {
            return QueryGetters.GetAllPortfoliosReport(userId, marketService, balanceService, aggregatePortfolioService);
        }

        [Subscribe]
        [Topic]
        public async Task<List<StockReport>> OnUpdateStockReports(
            [Service] IPortfolioService portfolioService,
            [EventMessage] int userId, int portfolioId)
        {
            return await QueryGetters.GetStockReports(userId, portfolioService, portfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<List<FondReport>> OnUpdateFondReports(
            [Service] IPortfolioService portfolioService,
            [EventMessage] int userId, int portfolioId)
        {
            return await QueryGetters.GetFondReports(userId, portfolioService, portfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<List<BondReport>> OnUpdateBondReports(
            [Service] IPortfolioService portfolioService,
            [EventMessage] int userId, int portfolioId)
        {
            return await QueryGetters.GetBondReports(userId, portfolioService, portfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<AssetPricesReport> OnUpdatePricesReport(
            [Service] IMarketService marketService,
            [EventMessage] int userId)
        {
            return await QueryGetters.GetAllAssetPricesReport(userId, marketService);
        }
    }
}
