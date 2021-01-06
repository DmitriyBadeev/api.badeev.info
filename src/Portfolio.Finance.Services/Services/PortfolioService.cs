using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly FinanceDataService _financeData;
        private readonly IBalanceService _balanceService;

        public PortfolioService(FinanceDataService financeData, IBalanceService balanceService)
        {
            _financeData = financeData;
            _balanceService = balanceService;
        }

        public IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios(int userId)
        {
            return _financeData.EfContext.Portfolios.Where(p => p.UserId == userId);
        }

        public async Task<OperationResult> CreatePortfolio(string name, int userId)
        {
            var portfolio = new Core.Entities.Finance.Portfolio()
            {
                Name = name,
                UserId = userId
            };

            var containsSameNamePortfolio = 
                await _financeData.EfContext.Portfolios.AnyAsync(p => p.Name == name && p.UserId == userId);
            
            if (containsSameNamePortfolio)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Порфель с таким именем у Вас уже существует"
                };
            }

            await _financeData.EfContext.Portfolios.AddAsync(portfolio);
            await _financeData.EfContext.SaveChangesAsync();

            return new OperationResult()
            {
                IsSuccess = true,
                Message = $"Портфель {name} создан успешно"
            };
        }

        public async Task<OperationResult> AddPaymentInPortfolio(int portfolioId, int userId, string ticket, int amount, 
            int paymentValue, DateTime date)
        {
            var portfolio = await _financeData.EfContext.Portfolios.FindAsync(portfolioId);
            
            var validationResult = CommonValidate(portfolioId, userId, portfolio);
            if (validationResult != null)
            {
                return validationResult;
            }
            
            var payment = new Payment()
            {
                Ticket = ticket,
                Amount = amount,
                Date = date,
                PaymentValue = paymentValue,
                PortfolioId = portfolio.Id,
                Portfolio = portfolio
            };

            await _financeData.EfContext.Payments.AddAsync(payment);
            await _financeData.EfContext.SaveChangesAsync();
            
            return new OperationResult()
            {
                IsSuccess = true,
                Message = $"Выплата для {ticket} произведена успешно"
            };
        }

        public async Task<OperationResult<List<Payment>>> GetPortfolioPayments(int portfolioId, int userId)
        {
            var portfolio = await _financeData.EfContext.Portfolios.FindAsync(portfolioId);

            var validationResult = CommonValidate<List<Payment>>(portfolioId, userId, portfolio);
            if (validationResult != null)
            {
                return validationResult;
            }

            var payments = await _financeData.EfContext.Payments
                .Where(p => p.PortfolioId == portfolioId)
                .ToListAsync();

            return new OperationResult<List<Payment>>()
            {
                IsSuccess = true,
                Message = $"Выплаты для портфеля {portfolio.Name}",
                Result = payments
            };
        }

        public async Task<OperationResult<int>> GetPortfolioPaymentProfit(int portfolioId, int userId)
        {
            var result = await GetPortfolioPayments(portfolioId, userId);

            if (!result.IsSuccess)
            {
                return new OperationResult<int>()
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                    Result = 0
                };
            }

            var payments = result.Result;

            var profit = payments.Aggregate(0, (sum, payment) => sum + payment.PaymentValue);
            
            return new OperationResult<int>()
            {
                IsSuccess = true,
                Message = "Дивидендная прибыль",
                Result = profit
            };
        }

        public async Task<OperationResult<double>> GetPortfolioPaymentProfitPercent(int portfolioId, int userId)
        {
            var result = await GetPortfolioPaymentProfit(portfolioId, userId);
            
            if (!result.IsSuccess)
            {
                return new OperationResult<double>()
                {
                    IsSuccess = result.IsSuccess,
                    Message = result.Message,
                    Result = 0
                };
            }

            var profit = result.Result;
            var investingSum = _balanceService.GetInvestSum(portfolioId, userId);

            var percent = FinanceHelpers.DivWithOneDigitRound(profit, investingSum);
            
            return new OperationResult<double>()
            {
                IsSuccess = true,
                Message = "Процент дивидендной прибыли",
                Result = percent
            };
        }

        private OperationResult CommonValidate(int portfolioId, int userId,
            Core.Entities.Finance.Portfolio portfolio)
        {
            var validationResult = CommonValidate<int>(portfolioId, userId, portfolio);

            if (validationResult == null) 
                return null;
            
            return new OperationResult()
            {
                Message = validationResult.Message,
                IsSuccess = validationResult.IsSuccess
            };
        }
        
        private OperationResult<TResult> CommonValidate<TResult>(int portfolioId, int userId, 
            Core.Entities.Finance.Portfolio portfolio)
        {
            if (portfolio == null)
            {
                return new OperationResult<TResult>()
                {
                    IsSuccess = false,
                    Message = $"Портфель с id={portfolioId} не найден"
                };
            }

            if (portfolio.UserId != userId)
            {
                return new OperationResult<TResult>()
                {
                    IsSuccess = false,
                    Message = $"Портфель с id={portfolioId} вам не принадлежит"
                };
            }

            return null;
        }
    }
}