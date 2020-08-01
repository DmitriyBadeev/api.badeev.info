using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;
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
