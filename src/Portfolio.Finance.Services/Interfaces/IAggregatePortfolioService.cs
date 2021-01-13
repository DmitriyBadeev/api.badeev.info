using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IAggregatePortfolioService
    {
        Task<OperationResult<List<Payment>>> AggregatePayments(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<ValuePercent>> AggregatePaymentProfit(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<ValuePercent>> AggregatePaperProfit(IEnumerable<int> portfolioIds, int userId);

        Task<OperationResult<int>> AggregateCost(IEnumerable<int> portfolioIds, int userId);

        IEnumerable<StockInfo> AggregateStocks(IEnumerable<int> portfolioIds, int userId);

        IEnumerable<FondInfo> AggregateFonds(IEnumerable<int> portfolioIds, int userId);

        IEnumerable<BondInfo> AggregateBonds(IEnumerable<int> portfolioIds, int userId);
    }
}