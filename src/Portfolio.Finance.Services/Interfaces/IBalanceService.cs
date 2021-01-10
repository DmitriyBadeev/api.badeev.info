using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IBalanceService
    {
        int GetBalance(int portfolioId);

        IEnumerable<CurrencyOperation> GetAllCurrencyOperations(int portfolioId);
        
        int GetAllInvestSum(int userId);

        int GetAllBalanceUser(int userId);

        Task<OperationResult> RefillBalance(int portfolioId, int price, DateTime date);

        Task<OperationResult> WithdrawalBalance(int portfolioId, int price, DateTime date);

        int GetInvestSum(int portfolioId, int userId);

        int GetAggregateInvestSum(IEnumerable<int> portfolioIds, int userId);
    }
}