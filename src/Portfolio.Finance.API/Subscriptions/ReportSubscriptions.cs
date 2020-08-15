using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Portfolio.Finance.API.Mutations;
using Portfolio.Finance.API.Queries;
using Portfolio.Finance.API.Queries.Response;
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
            [EventMessage] UserAndPortfolioIds ids)
        {
            return await QueryGetters.GetStockReports(ids.UserId, marketService, ids.PortfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<List<FondReport>> OnUpdateFondReports(
            [Service] IMarketService marketService,
            [EventMessage] UserAndPortfolioIds ids)
        {
            return await QueryGetters.GetFondReports(ids.UserId, marketService, ids.PortfolioId);
        }

        [Subscribe]
        [Topic]
        public async Task<List<BondReport>> OnUpdateBondReports(
            [Service] IMarketService marketService,
            [EventMessage] UserAndPortfolioIds ids)
        {
            return await QueryGetters.GetBondReports(ids.UserId, marketService, ids.PortfolioId);
        }
    }
}
