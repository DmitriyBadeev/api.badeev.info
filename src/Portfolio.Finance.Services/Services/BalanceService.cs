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

        public int GetAllBalanceUser(int userId)
        {
            var userPortfolios = _financeDataService.EfContext.Portfolios
                .Where(p => p.UserId == userId)
                .ToList();

            return userPortfolios.Aggregate(0, (total, portfolio) => total + GetBalance(portfolio.Id));
        }

        public IEnumerable<CurrencyOperation> GetAllCurrencyOperations(int portfolioId)
        {
            return _financeDataService.EfContext.CurrencyOperations.Where(o => o.PortfolioId == portfolioId);
        }

        public int GetAllInvestSum(int userId)
        {
            var operations = _financeDataService.EfContext.CurrencyOperations
                .Include(o => o.CurrencyAction)
                .Include(o => o.Portfolio)
                .Where(o => o.Portfolio.UserId == userId);

            var sum = 0;
            foreach (var currencyOperation in operations)
            {
                sum = ApplyCurrencyOperation(currencyOperation, sum);
            }

            return sum;
        }

        public int GetBalance(int portfolioId)
        {
            var balance = 0;

            var currencyOperations = _financeDataService.EfContext.CurrencyOperations
                .Where(o => o.PortfolioId == portfolioId)
                .Include(o => o.CurrencyAction);

            foreach (var currencyOperation in currencyOperations)
            {
                balance = ApplyCurrencyOperation(currencyOperation, balance);
            }

            var assetOperations = _financeDataService.EfContext.AssetOperations
                .Where(o => o.PortfolioId == portfolioId)
                .Include(o => o.AssetAction);

            foreach (var assetOperation in assetOperations)
            {
                balance = ApplyAssetOperation(assetOperation, balance);
            }

            return balance;
        }

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

            var currentBalance = GetBalance(portfolioId);
            if (price > currentBalance)
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
