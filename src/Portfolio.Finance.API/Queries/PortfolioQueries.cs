using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.API.Queries.Response;
using Portfolio.Finance.Services;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class PortfolioQueries
    {
        [Authorize]
        public IEnumerable<Core.Entities.Finance.Portfolio> GetPortfolios([CurrentUserIdGlobalState] int userId, 
            [Service] IPortfolioService portfolioService)
        {
            return portfolioService.GetPortfolios(userId);
        }

        [Authorize]
        public async Task<OperationResult<List<Payment>>> GetPortfolioPayments(
            [CurrentUserIdGlobalState] int userId,
            [Service] IPortfolioService portfolioService, 
            int portfolioId)
        {
            return await portfolioService.GetPortfolioPayments(portfolioId, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<List<Payment>>> AggregatePortfolioPayments(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            int[] portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePayments(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<ValuePercent>> AggregatePortfolioPaymentProfit(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePaymentProfit(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<ValuePercent>> AggregatePortfolioPaperProfit(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregatePaperProfit(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<int>> AggregatePortfolioCost(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            return await aggregatePortfolioService.AggregateCost(portfolioIds, userId);
        }
        
        [Authorize]
        public async Task<OperationResult<CostWithInvestSum>> AggregatePortfolioCostWithInvestSum(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            [Service] IBalanceService balanceService, 
            IEnumerable<int> portfolioIds)
        {
            var ids = portfolioIds.ToList();
            var cost = await aggregatePortfolioService.AggregateCost(ids, userId);

            if (!cost.IsSuccess)
            {
                return new OperationResult<CostWithInvestSum>()
                {
                    IsSuccess = cost.IsSuccess,
                    Message = cost.Message
                };
            }

            var investSum = balanceService.GetAggregateInvestSum(ids, userId);

            return new OperationResult<CostWithInvestSum>()
            {
                IsSuccess = true,
                Message = "Суммарная стоимость и суммарнаые инвестиции",
                Result = new CostWithInvestSum()
                {
                    Cost = cost.Result,
                    InvestSum = investSum
                }
            };
        }
        
        [Authorize]
        public async Task<List<StockReport>> AggregateStocks(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            var stocks = aggregatePortfolioService.AggregateStocks(portfolioIds, userId);

            return await GetStockReports(stocks);
        }
        
        [Authorize]
        public async Task<List<FondReport>> AggregateFonds(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            var fonds = aggregatePortfolioService.AggregateFonds(portfolioIds, userId);

            return await GetFondReports(fonds);
        }
        
        [Authorize]
        public async Task<List<BondReport>> AggregateBonds(
            [CurrentUserIdGlobalState] int userId,
            [Service] IAggregatePortfolioService aggregatePortfolioService, 
            IEnumerable<int> portfolioIds)
        {
            var bonds = aggregatePortfolioService.AggregateBonds(portfolioIds, userId);

            return await GetBondReports(bonds);
        }

        [Authorize]
        public string SecretData()
        {
            return "Secret";
        }

        public string Test()
        {
            return "Test";
        }

        private async Task<List<StockReport>> GetStockReports(IEnumerable<StockInfo> stocks)
        {
            var stockReports = new List<StockReport>();

            foreach (var stockInfo in stocks)
            {
                var name = await stockInfo.GetName();
                var price = await stockInfo.GetPrice();
                var percentChange = await stockInfo.GetPriceChange();
                var allPrice = await stockInfo.GetAllPrice();
                var paperProfit = await stockInfo.GetPaperProfit();
                var paperProfitPercent = await stockInfo.GetPaperProfitPercent();
                var updateTime = await stockInfo.GetUpdateTime();

                var stockReport = new StockReport()
                {
                    Name = name,
                    Ticket = stockInfo.Ticket,
                    Price = price,
                    PriceChange = FinanceHelpers.GetPriceDouble(percentChange),
                    AllPrice = FinanceHelpers.GetPriceDouble(allPrice),
                    BoughtPrice = FinanceHelpers.GetPriceDouble(stockInfo.BoughtPrice),
                    Amount = stockInfo.Amount,
                    PaidDividends = FinanceHelpers.GetPriceDouble(stockInfo.GetSumPayments()),
                    PaperProfit = FinanceHelpers.GetPriceDouble(paperProfit),
                    PaperProfitPercent = paperProfitPercent,
                    UpdateTime = updateTime,
                    NearestDividend = stockInfo.GetNearestPayment()
                };
                
                stockReports.Add(stockReport);
            }

            return stockReports;
        }

        private async Task<List<FondReport>> GetFondReports(IEnumerable<FondInfo> fonds)
        {
            var fondReports = new List<FondReport>();

            foreach (var fondInfo in fonds)
            {
                var name = await fondInfo.GetName();
                var price = await fondInfo.GetPrice();
                var percentChange = await fondInfo.GetPriceChange();
                var allPrice = await fondInfo.GetAllPrice();
                var paperProfit = await fondInfo.GetPaperProfit();
                var paperProfitPercent = await fondInfo.GetPaperProfitPercent();
                var updateTime = await fondInfo.GetUpdateTime();

                var fondReport = new FondReport()
                {
                    Name = name,
                    Ticket = fondInfo.Ticket,
                    Price = price,
                    PriceChange = FinanceHelpers.GetPriceDouble(percentChange),
                    AllPrice = FinanceHelpers.GetPriceDouble(allPrice),
                    BoughtPrice = FinanceHelpers.GetPriceDouble(fondInfo.BoughtPrice),
                    Amount = fondInfo.Amount,
                    PaperProfit = FinanceHelpers.GetPriceDouble(paperProfit),
                    PaperProfitPercent = paperProfitPercent,
                    UpdateTime = updateTime,
                };

                fondReports.Add(fondReport);
            }

            return fondReports;
        }

        private async Task<List<BondReport>> GetBondReports(IEnumerable<BondInfo> bonds)
        {
            var bondReports = new List<BondReport>();

            foreach (var bondInfo in bonds)
            {
                var name = await bondInfo.GetName();
                var price = await bondInfo.GetPrice();
                var percentChange = await bondInfo.GetPriceChange();
                var allPrice = await bondInfo.GetAllPrice();
                var paperProfit = await bondInfo.GetPaperProfit();
                var paperProfitPercent = await bondInfo.GetPaperProfitPercent();
                var updateTime = await bondInfo.GetUpdateTime();

                var bondReport = new BondReport()
                {
                    Name = name,
                    Ticket = bondInfo.Ticket,
                    Price = price,
                    PriceChange = FinanceHelpers.GetPriceDouble(percentChange),
                    AllPrice = FinanceHelpers.GetPriceDouble(allPrice),
                    BoughtPrice = FinanceHelpers.GetPriceDouble(bondInfo.BoughtPrice),
                    Amount = bondInfo.Amount,
                    PaidPayments = FinanceHelpers.GetPriceDouble(bondInfo.GetSumPayments()),
                    PaperProfit = FinanceHelpers.GetPriceDouble(paperProfit),
                    PaperProfitPercent = paperProfitPercent,
                    UpdateTime = updateTime,
                    NearestPayment = bondInfo.GetNearestPayment(),
                    HasAmortized = bondInfo.HasAmortized,
                    AmortizationDate = bondInfo.AmortizationDate
                };

                bondReports.Add(bondReport);
            }

            return bondReports;
        }
    }
}
