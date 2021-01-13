using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<OperationResult> CreatePortfolio(string name, int userId);

        IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios(int userId);

        Task<OperationResult> AddPaymentInPortfolio(int portfolioId, int userId, string ticket, int amount,
            int paymentValue, DateTime date);

        Task<OperationResult<List<Payment>>> GetPortfolioPayments(int portfolioId, int userId);

        Task<OperationResult<ValuePercent>> GetPortfolioPaymentProfit(int portfolioId, int userId);

        Task<OperationResult<ValuePercent>> GetPaperProfit(int portfolioId, int userId);

        Task<OperationResult<int>> GetCost(int portfolioId, int userId);

        Task<OperationResult<int>> GetPaperPrice(int portfolioId, int userId);

        IEnumerable<StockInfo> GetStocks(int portfolioId, int userId);
        
        IEnumerable<FondInfo> GetFonds(int portfolioId, int userId);
        
        IEnumerable<BondInfo> GetBonds(int portfolioId, int userId);
    }
}