using System.Linq;
using NUnit.Framework;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class MarketServiceTests
    {
        private AssetsFactory _assetFactory;
        private FinanceDataService _financeDataService;

        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            var stockMarketData = new StockMarketData(stockMarketAPI);

            TestHelpers.MockStockData(mockHttp);
            TestHelpers.MockDividendData(mockHttp);

            var context = TestHelpers.GetMockFinanceDbContext();
            TestHelpers.SeedOperations1(context);
            _financeDataService = new FinanceDataService(context);

            _assetFactory = new AssetsFactory(_financeDataService, stockMarketData);
            
        }

        [Test]
        public void GetAllProfitTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, 1);
            var profit = marketService.GetAllPaperProfit();
            Assert.AreEqual(-1042025, profit);
        }

        [Test]
        public void GetAllProfitTest__otherUser()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, 2);
            var profit = marketService.GetAllPaperProfit();
            Assert.AreEqual(21779 - 21430, profit);
        }

        [Test]
        public void GetAllPriceTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, 1);
            var price = marketService.GetAllPrice();
            Assert.AreEqual(407800 + 5 * 21779, price);
        }

        [Test]
        public void GetAllPaymentProfitTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, 1);
            var profit = marketService.GetAllPaymentProfit();

            Assert.AreEqual(11600, profit);
        }

        [Test]
        public void GetStocksTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, 1);
            var stocks1 = marketService.GetStocks(1);
            var stocks2 = marketService.GetStocks(2);

            Assert.AreEqual(2, stocks1.Count());
            Assert.AreEqual(1, stocks2.Count());
        }
    }
}