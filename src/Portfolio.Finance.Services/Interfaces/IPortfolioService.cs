using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<OperationResult> CreatePortfolio(string name, int userId);

        IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios(int userId);

        Task<OperationResult> AddPaymentInPortfolio(int portfolioId, int userId, string ticket, int amount,
            int paymentValue, DateTime date);

        Task<OperationResult<List<Payment>>> GetPortfolioPayments(int portfolioId, int userId);

        Task<OperationResult<int>> GetPortfolioPaymentProfit(int portfolioId, int userId);

        Task<OperationResult<double>> GetPortfolioPaymentProfitPercent(int portfolioId, int userId);
    }
}