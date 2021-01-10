using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAggregatePortfolioService
    {
        Task<OperationResult<List<Payment>>> AggregatePayments(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<ValuePercent>> AggregatePaymentProfit(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<ValuePercent>> AggregatePaperProfit(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<int>> AggregateCost(IEnumerable<int> portfolioIds, int userId);
    }
}