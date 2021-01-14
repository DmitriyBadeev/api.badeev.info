using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IBalanceService
    {
        Task<OperationResult<int>> AggregateBalance(IEnumerable<int> portfolioIds, int userId);
        Task<OperationResult<int>> GetBalance(int portfolioId, int userId);

        IEnumerable<CurrencyOperation> GetAllCurrencyOperations(int portfolioId);
        
        Task<OperationResult> RefillBalance(int portfolioId, int price, DateTime date);

        Task<OperationResult> WithdrawalBalance(int portfolioId, int price, DateTime date);

        int GetInvestSum(int portfolioId, int userId);

        int GetAggregateInvestSum(IEnumerable<int> portfolioIds, int userId);
    }
}