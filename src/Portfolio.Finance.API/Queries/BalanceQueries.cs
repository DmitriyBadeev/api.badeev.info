using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.Services;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class BalanceQueries
    {
        [Authorize]
        public double GetCurrentUserBalance([CurrentUserIdGlobalState] int userId,
            [Service] IMarketService marketService)
        {
            return FinanceHelpers.GetPriceDouble(marketService.GetUserBalanceWithPaidPayments(userId));
        }
    }
}