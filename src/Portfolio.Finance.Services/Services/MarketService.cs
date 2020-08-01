using System.Collections.Generic;
using System.Linq;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class MarketService : IMarketService
    {
        private readonly List<PortfolioData> _portfolios;

        public MarketService(FinanceDataService financeDataService, IAssetsFactory assetsFactory, int userId)
        {
            _portfolios = new List<PortfolioData>();
            var userPortfolios = financeDataService.EfContext.Portfolios.Where(p => p.UserId == userId);

            foreach (var userPortfolio in userPortfolios)
            {
                var portfolioData = new PortfolioData()
                {
                    Id = userPortfolio.Id,
                    Name = userPortfolio.Name,
                    UserId = userPortfolio.UserId,
                    Assets = assetsFactory.Create(userPortfolio.Id)
                };

                _portfolios.Add(portfolioData);
            }
        }

        public int GetAllPrice()
        {
            return _portfolios
                .Aggregate(0, (total, portfolio) => total + portfolio.Assets
                    .Aggregate(0, (totalPortfolio, asset) => totalPortfolio + asset.GetPrice() * asset.Amount));
        }

        public int GetAllPaperProfit()
        {
            return _portfolios
                .Aggregate(0, (total, portfolio) => total + portfolio.Assets
                    .Aggregate(0, (totalPortfolio, asset) => totalPortfolio + asset.GetPaperProfit()));
        }

        public int GetAllPaymentProfit()
        {
            return _portfolios
                .Aggregate(0, (total, portfolio) => total + portfolio.Assets
                    .Aggregate(0, (totalPortfolio, asset) => totalPortfolio + asset.GetPaidPayments()
                        .Aggregate(0, (totalPayment, payment) => totalPayment + payment.PaymentValue)));
        }

        public IEnumerable<StockInfo> GetStocks(int portfolioId)
        {
            var portfolio = _portfolios.Find(p => p.Id == portfolioId);

            foreach (var asset in portfolio.Assets)
            {
                var type = asset.GetType();

                if (type.Name == "StockInfo")
                    yield return (StockInfo)asset;
            }
        }
    }
}
