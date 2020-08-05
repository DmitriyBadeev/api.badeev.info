using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.API.Queries.Response;
using Portfolio.Finance.Services;
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
            var allCost = FinanceHelpers.GetPriceDouble(marketService.GetAllCost(userId));
            var allProfit = FinanceHelpers.GetPriceDouble(marketService.GetAllPaperProfit(userId));
            var allPaymentProfit = FinanceHelpers.GetPriceDouble(marketService.GetAllPaymentProfit(userId));
            var allInvestSum = FinanceHelpers.GetPriceDouble(balanceService.GetAllInvestSum(userId));

            return new AllPortfoliosReport()
            {
                AllCost = allCost,
                AllProfit = allProfit,
                AllPaymentProfit = allPaymentProfit,
                AllInvestSum = allInvestSum
            };
        }
    }
}