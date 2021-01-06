using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAggregatePortfolioService
    {
        Task<OperationResult<List<Payment>>> AggregatePayments(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<int>> AggregatePaymentProfit(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<double>> AggregatePaymentProfitPercent(IEnumerable<int> portfolioIds, int userId);
    }
}