using System.Collections.Generic;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class PortfolioQueries
    {
        [Authorize]
        public IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios([CurrentUserIdGlobalState] int userId, 
            [Service] IPortfolioService portfolioService)
        {
            return portfolioService.GetPortfolios(userId);
        }

        [Authorize]
        public string SecretData()
        {
            return "Secret";
        }

        public string Test()
        {
            return "Test";
        }
    }
}
