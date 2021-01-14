using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class BalanceQueries
    {
        [Authorize]
        public async Task<OperationResult<int>> AggregateBalance([CurrentUserIdGlobalState] int userId,
            [Service] IBalanceService balanceService, IEnumerable<int> portfolioIds)
        {
            return await balanceService.AggregateBalance(portfolioIds, userId);
        }
        
        [Authorize]
        public int GetAggregateInvestSum([CurrentUserIdGlobalState] int userId,
            [Service] IBalanceService balanceService, IEnumerable<int> portfolioIds)
        {
            return balanceService.GetAggregateInvestSum(portfolioIds, userId);
        }
    }
}