using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.API.Queries.Response;
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
        public async Task<OperationResult<ValuePercent>> AggregatePortfolioPaymentProfit(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePaymentProfit(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<ValuePercent>> AggregatePortfolioPaperProfit(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePaperProfit(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<int>> AggregatePortfolioCost(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregateCost(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<CostWithInvestSum>> AggregatePortfolioCostWithInvestSum(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            [Service] IBalanceService balanceService, 
            IEnumerable<int> portfolioIds)
        {
            var ids = portfolioIds.ToList();
            var cost = await aggregatePortfolioService.AggregateCost(ids, userId);

            if (!cost.IsSuccess)
            {
                return new OperationResult<CostWithInvestSum>()
                {
                    IsSuccess = cost.IsSuccess,
                    Message = cost.Message
                };
            }

            var investSum = balanceService.GetAggregateInvestSum(ids, userId);

            return new OperationResult<CostWithInvestSum>()
            {
                IsSuccess = true,
                Message = "Суммарная стоимость и суммарнаые инвестиции",
                Result = new CostWithInvestSum()
                {
                    Cost = cost.Result,
                    InvestSum = investSum
                }
            };
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
