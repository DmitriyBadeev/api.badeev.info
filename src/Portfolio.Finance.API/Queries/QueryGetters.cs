using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Finance.API.Queries.Response;
using Portfolio.Finance.Services;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;

namespace Portfolio.Finance.API.Queries
{
    public static class QueryGetters
    {
        public static AllPortfoliosReport GetAllPortfoliosReport(int userId, IMarketService marketService, IBalanceService balanceService)
        {
            var allCost = FinanceHelpers.GetPriceDouble(marketService.GetAllCost(userId));
            var allPaperProfit = FinanceHelpers.GetPriceDouble(marketService.GetAllPaperProfit(userId));
            var allPaperProfitPercent = marketService.GetPercentOfPaperProfit(userId);
            var allPaymentProfit = FinanceHelpers.GetPriceDouble(marketService.GetAllPaymentProfit(userId));
            var allPaymentProfitPercent = marketService.GetPercentOfPaymentProfit(userId);
            var allInvestSum = FinanceHelpers.GetPriceDouble(balanceService.GetAllInvestSum(userId));
            var allBalance = FinanceHelpers.GetPriceDouble(marketService.GetUserBalanceWithPaidPayments(userId));

            return new AllPortfoliosReport()
            {
                AllCost = allCost,
                AllPaperProfit = allPaperProfit,
                AllPaperProfitPercent = allPaperProfitPercent,
                AllPaymentProfit = allPaymentProfit,
                AllPaymentProfitPercent = allPaymentProfitPercent,
                AllInvestSum = allInvestSum,
                AllUserBalance = allBalance
            };
        }

        public static async Task<List<StockReport>> GetStockReports(int userId, IMarketService marketService, int portfolioId)
        {
            var stocks = marketService.GetStocks(portfolioId, userId);

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

        public static async Task<List<FondReport>> GetFondReports(int userId, IMarketService marketService, int portfolioId)
        {
            var fonds = marketService.GetFonds(portfolioId, userId);

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

        public static async Task<List<BondReport>> GetBondReports(int userId, IMarketService marketService, int portfolioId)
        {
            var bonds = marketService.GetBonds(portfolioId, userId);

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

        public static async Task<AssetPricesReport> GetAllAssetPricesReport(int userId, IMarketService marketService)
        {
            var assetPrices = await marketService.GetAllAssetPrices(userId);

            return new AssetPricesReport()
            {
                StockPrice = FinanceHelpers.GetPriceDouble(assetPrices.StockPrice),
                FondPrice = FinanceHelpers.GetPriceDouble(assetPrices.FondPrice),
                BondPrice = FinanceHelpers.GetPriceDouble(assetPrices.BondPrice)
            };
        }

        public static IEnumerable<PaymentDataReport> GetAllFuturePaymentsReport(int userId, IMarketService marketService)
        {
            var payments = marketService.GetAllFuturePayments(userId);

            return payments
                .Select(p => new PaymentDataReport()
                {
                    Name = p.Name,
                    Ticket = p.Ticket,
                    PaymentValue = FinanceHelpers.GetPriceDouble(p.PaymentValue),
                    Amount = p.Amount,
                    AllPayment = FinanceHelpers.GetPriceDouble(p.AllPayment),
                    CurrencyId = p.CurrencyId,
                    RegistryCloseDate = p.RegistryCloseDate
                })
                .OrderBy(p => p.RegistryCloseDate);
        }

        public static IEnumerable<CommonMarketQuote> GetMarketQuotes(IMarketQuotesService quotesService)
        {
            return quotesService.GetMarketQuotes();
        }

        public static async Task<AssetData> AssetReport(ISearchService searchService, string ticket, int userId)
        {
            return await searchService.GetAssetData(ticket, userId);
        }

        public static async Task<SearchData> SearchAsset(ISearchService searchService, string ticket)
        {
            return await searchService.Search(ticket);
        }
    }
}
