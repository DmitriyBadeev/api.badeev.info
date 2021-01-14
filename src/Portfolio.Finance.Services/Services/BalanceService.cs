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
    public class BalanceService : IBalanceService
    {
        private readonly FinanceDataService _financeDataService;

        public BalanceService(FinanceDataService financeDataService)
        {
            _financeDataService = financeDataService;
        }

        public IEnumerable<CurrencyOperation> GetAllCurrencyOperations(int portfolioId)
        {
            return _financeDataService.EfContext.CurrencyOperations.Where(o => o.PortfolioId == portfolioId);
        }

        public int GetAggregateInvestSum(IEnumerable<int> portfolioIds, int userId)
        {
            var operations = _financeDataService.EfContext.CurrencyOperations
                .Include(o => o.CurrencyAction)
                .Include(o => o.Portfolio)
                .Where(o => portfolioIds.Any(id => id == o.Portfolio.Id) && o.Portfolio.UserId == userId);
            
            return GetOperationsSum(operations);
        }
        
        public int GetInvestSum(int portfolioId, int userId)
        {
            var operations = _financeDataService.EfContext.CurrencyOperations
                .Include(o => o.CurrencyAction)
                .Include(o => o.Portfolio)
                .Where(o => o.Portfolio.UserId == userId && o.Portfolio.Id == portfolioId);
            
            return GetOperationsSum(operations);
        }

        public async Task<OperationResult<int>> AggregateBalance(IEnumerable<int> portfolioIds, int userId)
        {
            var balance = 0;

            var ids = portfolioIds.ToList();
            foreach (var portfolioId in ids)
            {
                var portfolioBalance = await GetBalance(portfolioId, userId);

                if (!portfolioBalance.IsSuccess)
                {
                    return portfolioBalance;
                }

                balance += portfolioBalance.Result;
            }

            return new OperationResult<int>()
            {
                IsSuccess = true,
                Message = $"Баланс портфелей с id={string.Join(", ", ids)}",
                Result = balance
            };
        }
        
        public async Task<OperationResult<int>> GetBalance(int portfolioId, int userId)
        {
            var portfolio = await _financeDataService.EfContext.Portfolios.FindAsync(portfolioId);

            if (portfolio == null)
            {
                return new OperationResult<int>()
                {
                    IsSuccess = false,
                    Message = "Порфель не найден"
                };
            }

            if (portfolio.UserId != userId)
            {
                return new OperationResult<int>()
                {
                    IsSuccess = false,
                    Message = "Портфель не принадлежит пользователю"
                };
            }
            
            var currencyOperations = _financeDataService.EfContext.CurrencyOperations
                .Where(o => o.PortfolioId == portfolioId)
                .Include(o => o.CurrencyAction);

            var balance = Enumerable.Aggregate(currencyOperations, 0, 
                (current, currencyOperation) => ApplyCurrencyOperation(currencyOperation, current));

            var assetOperations = _financeDataService.EfContext.AssetOperations
                .Where(o => o.PortfolioId == portfolioId)
                .Include(o => o.AssetAction);

            var payments = await _financeDataService.EfContext.Payments
                .Where(p => p.PortfolioId == portfolioId)
                .ToListAsync();
                
            var paymentProfit = payments    
                .Aggregate(0, (sum, payment) => sum + payment.PaymentValue);

            balance = payments.Aggregate(balance, (current, payment) => current + payment.PaymentValue);
            
            balance = Enumerable.Aggregate(assetOperations, balance, 
                (current, assetOperation) => ApplyAssetOperation(assetOperation, current));

            return new OperationResult<int>()
            {
                IsSuccess = true,
                Message = $"Баланс портфеля {portfolio.Name}",
                Result = balance
            };
        }
        
        //TODO Проверка на нужного пользователя
        public async Task<OperationResult> RefillBalance(int portfolioId, int price, DateTime date)
        {
            var portfolio = await _financeDataService.EfContext.Portfolios.FindAsync(portfolioId);
            var refillAction =
                await _financeDataService.EfContext.CurrencyActions.FirstOrDefaultAsync(a =>
                    a.Name == SeedFinanceData.REFILL_ACTION);

            if (portfolio == null)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Портфель не найден"
                };
            }

            if (price < 0)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Нельзя пополнить на отрицательную сумму"
                };
            }

            var currencyOperation = new CurrencyOperation()
            {
                Portfolio = portfolio,
                PortfolioId = portfolioId,
                CurrencyAction = refillAction,
                CurrencyActionId = refillAction.Id,
                CurrencyId = SeedFinanceData.RUB_CURRENCY_ID,
                CurrencyName = SeedFinanceData.RUB_CURRENCY_NAME,
                Date = date,
                Price = price
            };

            await _financeDataService.EfContext.CurrencyOperations.AddAsync(currencyOperation);
            await _financeDataService.EfContext.SaveChangesAsync();

            return new OperationResult()
            {
                IsSuccess = true,
                Message = $"Баланс пополнен на {FinanceHelpers.GetPriceDouble(price)} ₽ успешно"
            };
        }

        public async Task<OperationResult> WithdrawalBalance(int portfolioId, int price, DateTime date)
        {
            var portfolio = await _financeDataService.EfContext.Portfolios.FindAsync(portfolioId);
            var withdrawalAction =
                await _financeDataService.EfContext.CurrencyActions.FirstOrDefaultAsync(a =>
                    a.Name == SeedFinanceData.WITHDRAWAL_ACTION);

            if (portfolio == null)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Портфель не найден"
                };
            }

            if (price < 0)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Нельзя вывести отрицательную сумму"
                };
            }

            var currentBalanceResult = await GetBalance(portfolioId, portfolio.UserId);
            if (price > currentBalanceResult.Result)
            {
                return new OperationResult()
                {
                    IsSuccess = false,
                    Message = "Недостаточно средств"
                };
            }

            var currencyOperation = new CurrencyOperation()
            {
                Portfolio = portfolio,
                PortfolioId = portfolioId,
                CurrencyAction = withdrawalAction,
                CurrencyActionId = withdrawalAction.Id,
                CurrencyId = SeedFinanceData.RUB_CURRENCY_ID,
                CurrencyName = SeedFinanceData.RUB_CURRENCY_NAME,
                Date = date,
                Price = price
            };

            await _financeDataService.EfContext.CurrencyOperations.AddAsync(currencyOperation);
            await _financeDataService.EfContext.SaveChangesAsync();

            return new OperationResult()
            {
                IsSuccess = true,
                Message = "Средства выведены успешно"
            };
        }

        private int GetOperationsSum(IQueryable<CurrencyOperation> operations)
        {
            var sum = 0;
            foreach (var currencyOperation in operations)
            {
                sum = ApplyCurrencyOperation(currencyOperation, sum);
            }

            return sum;
        }
        
        private int ApplyCurrencyOperation(CurrencyOperation operation, int balance)
        {
            if (operation.CurrencyAction.Name == SeedFinanceData.REFILL_ACTION)
            {
                balance += operation.Price;
            }

            if (operation.CurrencyAction.Name == SeedFinanceData.WITHDRAWAL_ACTION)
            {
                balance -= operation.Price;
            }

            return balance;
        }

        private int ApplyAssetOperation(AssetOperation operation, int balance)
        {
            if (operation.AssetAction.Name == SeedFinanceData.BUY_ACTION)
            {
                balance -= operation.PaymentPrice;
            }

            if (operation.AssetAction.Name == SeedFinanceData.SELL_ACTION)
            {
                balance += operation.PaymentPrice;
            }

            return balance;
        }
    }
}
