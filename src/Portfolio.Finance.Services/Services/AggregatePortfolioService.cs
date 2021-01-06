using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class AggregatePortfolioService : IAggregatePortfolioService
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IBalanceService _balanceService;

        public AggregatePortfolioService(IPortfolioService portfolioService, IBalanceService balanceService)
        {
            _portfolioService = portfolioService;
            _balanceService = balanceService;
        }

        public async Task<OperationResult<List<Payment>>> AggregatePayments(IEnumerable<int> portfolioIds, int userId)
        {
            var payments = new List<Payment>();

            var ids = portfolioIds.ToList();
            foreach (var portfolioId in ids)
            {
                var result = await _portfolioService.GetPortfolioPayments(portfolioId, userId);

                if (!result.IsSuccess)
                {
                    return result;
                }
                
                payments.AddRange(result.Result);
            }

            return new OperationResult<List<Payment>>()
            {
                IsSuccess = true,
                Message = $"Выплаты для портфелей(я) c id={string.Join(", ", ids)}",
                Result = payments
            };
        }

        public async Task<OperationResult<int>> AggregatePaymentProfit(IEnumerable<int> portfolioIds, int userId)
        {
            var profit = 0;
            
            var ids = portfolioIds.ToList();
            foreach (var portfolioId in ids)
            {
                var result = await _portfolioService.GetPortfolioPaymentProfit(portfolioId, userId);

                if (!result.IsSuccess)
                {
                    return result;
                }

                profit += result.Result;
            }

            return new OperationResult<int>()
            {
                IsSuccess = true,
                Message = $"Суммарная дивидендная прибыль портфелей(я) c id={string.Join(", ", ids)}",
                Result = profit
            };
        }

        public async Task<OperationResult<double>> AggregatePaymentProfitPercent(IEnumerable<int> portfolioIds, int userId)
        {
            var sumProfit = 0;
            var sumInvest = 0;
            
            var ids = portfolioIds.ToList();
            foreach (var portfolioId in ids)
            {
                var resultProfit = await _portfolioService.GetPortfolioPaymentProfit(portfolioId, userId);
                var resultInvestSum = _balanceService.GetInvestSum(portfolioId, userId);

                if (!resultProfit.IsSuccess)
                {
                    return new OperationResult<double>()
                    {
                        IsSuccess = resultProfit.IsSuccess,
                        Message = resultProfit.Message
                    };
                }

                sumProfit += resultProfit.Result;
                sumInvest += resultInvestSum;
            }

            var percent = FinanceHelpers.DivWithOneDigitRound(sumProfit, sumInvest);
            
            return new OperationResult<double>()
            {
                IsSuccess = true,
                Message = $"Процент дивидендной прибыли портфелей(я) c id={string.Join(", ", ids)}",
                Result = percent
            };
        }
    }
}