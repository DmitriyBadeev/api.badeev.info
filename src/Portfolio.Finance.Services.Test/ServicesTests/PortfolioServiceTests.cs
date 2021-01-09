using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class PortfolioServiceTests
    {
        private PortfolioService _portfolioService;
        private FinanceDataService _financeDataService;
        private BalanceService _balanceService;

        [SetUp]
        public async Task Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();

            var stockMarketApi = new StockMarketAPI(client);
            var stockMarketData = new StockMarketData(stockMarketApi);
            
            var context = TestHelpers.GetMockFinanceDbContext();
            _financeDataService = new FinanceDataService(context);
          
            var assetFactory = new AssetsFactory(_financeDataService, stockMarketData);
            _balanceService = new BalanceService(_financeDataService);
            TestHelpers.SeedApp(context);
            _portfolioService = new PortfolioService(_financeDataService, _balanceService, assetFactory);

            TestHelpers.MockStockData(mockHttp);
            TestHelpers.MockFondData(mockHttp);
            TestHelpers.MockBondData(mockHttp);
            
            TestHelpers.SeedOperations2(context);
        }

        [Test]
        public async Task CreateTest()
        {
            var name = "Тестовый портфель 2";
            var result = await _portfolioService.CreatePortfolio(name, 1);

            Assert.IsTrue(result.IsSuccess);

            var containsPortfolio = await 
                _financeDataService.EfContext.Portfolios.AnyAsync(p => p.Name == name && p.UserId == 1);
            
            Assert.IsTrue(containsPortfolio);
        }

        [Test]
        public async Task CreateTest__SameName()
        {
            var name = "Тестовый портфель 2";

            var result1 = await _portfolioService.CreatePortfolio(name, 1);
            Assert.IsTrue(result1.IsSuccess);

            var result2 = await _portfolioService.CreatePortfolio(name, 1);
            Assert.IsFalse(result2.IsSuccess);

            var result3 = await _portfolioService.CreatePortfolio(name, 2);
            Assert.IsTrue(result3.IsSuccess);
        }

        [Test]
        public async Task GetPortfoliosTest()
        {
            var name = "Тестовый портфель 2";
            var result1 = await _portfolioService.CreatePortfolio(name,1);
            Assert.IsTrue(result1.IsSuccess);

            var result2 = await _portfolioService.CreatePortfolio(name, 2);
            Assert.IsTrue(result2.IsSuccess);

            var portfolios = _portfolioService.GetPortfolios(1);

            Assert.AreEqual(3, portfolios.Count());
        }

        [Test]
        public async Task AddPaymentInPortfolio()
        {
            var result1 = await _portfolioService.AddPaymentInPortfolio(10, 1,"SBER", 30, 
                60000, DateTime.Now);
            
            var result2 = await _portfolioService.AddPaymentInPortfolio(999, 1, "SBER", 30, 
                60000, DateTime.Now);
            
            var result3 = await _portfolioService.AddPaymentInPortfolio(10, 2, "SBER", 30, 
                60000, DateTime.Now);
            
            Assert.IsTrue(result1.IsSuccess, "Некорректное добавление выплаты");
            Assert.IsFalse(result2.IsSuccess, "Добавление выплаты в несуществеющий портфель");
            Assert.IsFalse(result3.IsSuccess, "Добавление выплаты в портфель, который не принадлежит пользователю");
        }

        [Test]
        public async Task GetPortfolioPayments()
        {
            var result1 = await _portfolioService.GetPortfolioPayments(10, 1);
            var result2 = await _portfolioService.GetPortfolioPayments(10, 2);
            
            Assert.IsTrue(result1.IsSuccess, "Неуспешное выполнение операции");
            Assert.IsFalse(result2.IsSuccess, "Получение выплат у чужого пользователя");
            Assert.AreEqual(3, result1.Result.Count, "Неверное количество выплат");
        }

        [Test]
        public async Task GetPortfolioPaymentProfit()
        {
            var profit1 = await _portfolioService.GetPortfolioPaymentProfit(10, 1);
            var profit2 = await _portfolioService.GetPortfolioPaymentProfit(11, 1);
            var profit3 = await _portfolioService.GetPortfolioPaymentProfit(13, 1);

            Assert.IsTrue(profit1.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(115000, profit1.Result.Value, "Неверная прибыль");
            Assert.AreEqual(5.8, profit1.Result.Percent, "Неверный процент");
            
            Assert.IsTrue(profit2.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(5000, profit2.Result.Value, "Неверная прибыль");
            Assert.AreEqual(0.5, profit2.Result.Percent, "Неверный процент");
            
            Assert.IsFalse(profit3.IsSuccess, "Прибыль в чужом портфеле");
        }

        [Test]
        public async Task GetPaperProfit()
        {
            var profit1 = await _portfolioService.GetPaperProfit(10, 1);
            var profit2 = await _portfolioService.GetPaperProfit(11, 1);
            var profit3 = await _portfolioService.GetPaperProfit(12, 2);
            
            var profit4 = await _portfolioService.GetPaperProfit(12, 1);
            
            Assert.IsTrue(profit1.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(34720 * 2 + 26580 * 2 + 20000 + 6283 - 100000, profit1.Result, "Неверная прибыль");
            
            Assert.IsTrue(profit2.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(26580, profit2.Result, "Неверная прибыль");
            
            Assert.IsTrue(profit3.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(34720, profit3.Result, "Неверная прибыль");
            
            Assert.IsFalse(profit4.IsSuccess, "Прибыль в чужом портфеле");
        }
    }
}