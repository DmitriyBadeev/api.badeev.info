using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
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
        public async Task<OperationResult<List<Payment>>> GetPortfolioPayments(
            [CurrentUserIdGlobalState] int userId,
            [Service] IPortfolioService portfolioService, 
            int portfolioId)
        {
            return await portfolioService.GetPortfolioPayments(portfolioId, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<List<Payment>>> AggregatePortfolioPayments(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            int[] portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePayments(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<int>> AggregatePortfolioPaymentProfit(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePaymentProfit(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<double>> AggregatePortfolioPaymentProfitPercent(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePaymentProfitPercent(portfolioIds, userId);
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
