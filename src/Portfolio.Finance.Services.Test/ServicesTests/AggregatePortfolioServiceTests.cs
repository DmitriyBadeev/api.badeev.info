using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class AggregatePortfolioServiceTests
    {
        private IAggregatePortfolioService _aggregatePortfolioService;
        private FinanceDataService _financeDataService;
        
        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();
            
            var context = TestHelpers.GetMockFinanceDbContext(); 
            _financeDataService = new FinanceDataService(context);
            var balanceService = new BalanceService(_financeDataService);
            TestHelpers.SeedApp(context);
            
            var stockMarketApi = new StockMarketAPI(client);
            var stockMarketData = new StockMarketData(stockMarketApi);
            var assetFactory = new AssetsFactory(_financeDataService, stockMarketData);
            
            var portfolioService = new PortfolioService(_financeDataService, balanceService, assetFactory);
            _aggregatePortfolioService = new AggregatePortfolioService(portfolioService, balanceService);
            
            TestHelpers.MockStockData(mockHttp);
            TestHelpers.MockFondData(mockHttp);
            TestHelpers.MockBondData(mockHttp);
            
            TestHelpers.SeedOperations2(context);
        }

        [Test]
        public async Task AggregatePayments()
        {
            var result1 = await _aggregatePortfolioService.AggregatePayments(new[] {10, 11}, 1);
            var result2 = await _aggregatePortfolioService.AggregatePayments(new[] {10, 11, 12}, 1);
            var result3 = await _aggregatePortfolioService.AggregatePayments(new[] {10}, 1);
            var result4 = await _aggregatePortfolioService.AggregatePayments(new[] {12}, 2);
            var result5 = await _aggregatePortfolioService.AggregatePayments(new[] {12}, 1);
            
            Assert.IsTrue(result1.IsSuccess);
            Assert.AreEqual(4, result1.Result.Count);
            
            Assert.IsTrue(result3.IsSuccess);
            Assert.AreEqual(3, result3.Result.Count);

            Assert.IsTrue(result4.IsSuccess);
            Assert.AreEqual(1, result4.Result.Count);
            
            Assert.IsFalse(result2.IsSuccess, "Считается портфель чужого пользователя");
            Assert.IsFalse(result5.IsSuccess, "Считается портфель чужого пользователя");
        }

        [Test]
        public async Task AggregatePaymentProfit()
        {
            var result1 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {10, 11}, 1);
            var result2 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {10, 11, 12}, 1);
            var result3 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {10}, 1);
            var result4 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {12}, 2);
            var result5 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {12}, 1);
            
            Assert.IsTrue(result1.IsSuccess);
            Assert.AreEqual(120000, result1.Result.Value);
            Assert.AreEqual(4, result1.Result.Percent);
            
            Assert.IsTrue(result3.IsSuccess);
            Assert.AreEqual(115000, result3.Result.Value);
            Assert.AreEqual(5.8, result3.Result.Percent);
            
            Assert.IsTrue(result4.IsSuccess);
            Assert.AreEqual(10000, result4.Result.Value);
            Assert.AreEqual(0.7, result4.Result.Percent);
            
            Assert.IsFalse(result2.IsSuccess, "Считается портфель чужого пользователя");
            Assert.IsFalse(result5.IsSuccess, "Считается портфель чужого пользователя");
        }

        [Test]
        public async Task AggregatePaperProfit()
        {
            var result1 = await _aggregatePortfolioService.AggregatePaperProfit(new[] {10, 11}, 1);
            var result2 = await _aggregatePortfolioService.AggregatePaperProfit(new[] {10, 11, 12}, 1);
            var result3 = await _aggregatePortfolioService.AggregatePaperProfit(new[] {10}, 1);
            var result4 = await _aggregatePortfolioService.AggregatePaperProfit(new[] {12}, 2);
            var result5 = await _aggregatePortfolioService.AggregatePaperProfit(new[] {12}, 1);
            
            Assert.IsTrue(result1.IsSuccess);
            Assert.AreEqual(34720 * 2 + 26580 * 2 + 20000 + 6283 - 100000 + 26580, result1.Result.Value);
            Assert.AreEqual(
                FinanceHelpers.DivWithOneDigitRound(34720 * 2 + 26580 * 2 + 20000 + 6283 - 100000 + 26580, 3000000), 
                result1.Result.Percent);
            
            Assert.IsTrue(result3.IsSuccess);
            Assert.AreEqual(34720 * 2 + 26580 * 2 + 20000 + 6283 - 100000, result3.Result.Value);
            Assert.AreEqual(
                FinanceHelpers.DivWithOneDigitRound(34720 * 2 + 26580 * 2 + 20000 + 6283 - 100000, 2000000),
                result3.Result.Percent);
            
            Assert.IsTrue(result4.IsSuccess);
            Assert.AreEqual(34720, result4.Result.Value);
            Assert.AreEqual(FinanceHelpers.DivWithOneDigitRound(34720, 1500000), result4.Result.Percent);
            
            Assert.IsFalse(result2.IsSuccess, "Считается портфель чужого пользователя");
            Assert.IsFalse(result5.IsSuccess, "Считается портфель чужого пользователя");
        }

        [Test]
        public async Task AggregateCost()
        {
            var result1 = await _aggregatePortfolioService.AggregateCost(new[] {10, 11}, 1);
            var result2 = await _aggregatePortfolioService.AggregateCost(new[] {10, 11, 12}, 1);
            var result3 = await _aggregatePortfolioService.AggregateCost(new[] {10}, 1);
            var result4 = await _aggregatePortfolioService.AggregateCost(new[] {12}, 2);
            var result5 = await _aggregatePortfolioService.AggregateCost(new[] {12}, 1);
            
            Assert.IsTrue(result1.IsSuccess);
            Assert.AreEqual(434720 * 2 + 22658 * 20 + 101840 + 106283 + 115000 + 600000 - 81840 + 22658 * 10 + 5000 + 800000,
                result1.Result);

            Assert.IsTrue(result3.IsSuccess);
            Assert.AreEqual(434720 * 2 + 22658 * 20 + 101840 + 106283 + 115000 + 600000 - 81840, result3.Result);
            
            Assert.IsTrue(result4.IsSuccess);
            Assert.AreEqual(434720 + 10000 + 1100000, result4.Result);
            
            Assert.IsFalse(result2.IsSuccess, "Считается портфель чужого пользователя");
            Assert.IsFalse(result5.IsSuccess, "Считается портфель чужого пользователя");
        }
    }
}