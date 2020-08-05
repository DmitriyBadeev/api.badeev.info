using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Mutations
{
    [ExtendObjectType(Name = "Mutations")]
    public class PortfolioMutations
    {
        [Authorize]
        public async Task<OperationResult> CreatePortfolio([CurrentUserIdGlobalState] int userId,
            [Service] IPortfolioService portfolioService, string name)
        {
            var result = await portfolioService.CreatePortfolio(name, userId);

            return result;
        }
    }
}