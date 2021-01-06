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
        public AllPortfoliosReport GetAllPortfoliosReport(
            [CurrentUserIdGlobalState] int userId, 
            [Service] IMarketService marketService, 
            [Service] IBalanceService balanceService, 
            [Service] IAggregatePortfolioService aggregatePortfolioService)
        {
            return QueryGetters.GetAllPortfoliosReport(userId, marketService, balanceService, aggregatePortfolioService);
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
        public async Task<AssetPricesReport> GetAllAssetPricesReport([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService)
        {
            return await QueryGetters.GetAllAssetPricesReport(userId, marketService);
        }

        [Authorize]
        public IEnumerable<PaymentDataReport> GetAllFuturePaymentsReport([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService)
        {
            return QueryGetters.GetAllFuturePaymentsReport(userId, marketService);
        }

        [Authorize]
        public IEnumerable<CommonMarketQuote> GetMarketQuotes([Service] IMarketQuotesService quotesService)
        {
            return QueryGetters.GetMarketQuotes(quotesService);
        }

        [Authorize]
        public async Task<SearchData> SearchAsset([Service] ISearchService searchService, string ticket)
        {
            return await QueryGetters.SearchAsset(searchService, ticket);
        }

        [Authorize]
        public async Task<AssetData> AssetReport([CurrentUserIdGlobalState] int userId, 
            [Service] ISearchService searchService, string ticket)
        {
            return  await QueryGetters.AssetReport(searchService, ticket, userId);
        }
    }
}