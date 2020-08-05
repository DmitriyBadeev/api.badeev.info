using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
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
        private IBalanceService _balanceService;

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
            _balanceService = new BalanceService(_financeDataService);
        }

        [Test]
        public void GetAllProfitTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var profit = marketService.GetAllPaperProfit(1);
            Assert.AreEqual(-1010710, profit);
        }

        [Test]
        public void GetAllProfitTest__otherUser()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var profit = marketService.GetAllPaperProfit(2);
            Assert.AreEqual(22658 - 21430, profit);
        }

        [Test]
        public void GetAllPriceTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var price = marketService.GetAllPaperPrice(1);
            Assert.AreEqual(548010, price);
        }

        [Test]
        public void GetAllPaymentProfitTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var profit = marketService.GetAllPaymentProfit(1);

            Assert.AreEqual(11600, profit);
        }

        [Test]
        public void GetStocksTest()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var stocks1 = marketService.GetStocks(1, 1);
            var stocks2 = marketService.GetStocks(1, 2);

            Assert.AreEqual(2, stocks1.Count());
            Assert.AreEqual(1, stocks2.Count());
        }

        [Test]
        public async Task BuyAsset()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result = await marketService.BuyAsset(1, "MTSS", 2000, 10,
                stockType.Id, DateTime.Now);
            Assert.IsTrue(result.IsSuccess);

            var stock = marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "MTSS");
            Assert.IsNotNull(stock);
        }

        [Test]
        public async Task BuyAsset__invalidData()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result1 = await marketService.BuyAsset(-1, "MTSS", 2000, 10,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result1.IsSuccess);

            var result2 = await marketService.BuyAsset(1, "MTSS", 99999999, 10,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result2.IsSuccess);

            var result3 = await marketService.BuyAsset(1, "MTSS", 2000, -1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result3.IsSuccess);

            var result4 = await marketService.BuyAsset(1, "MTSS", 2000, 10,
                -1, DateTime.Now);
            Assert.IsFalse(result4.IsSuccess);

            var result5 = await marketService.BuyAsset(1, "MTSS", -1, 10,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result5.IsSuccess);

            var stock = marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "MTSS");
            Assert.IsNull(stock);
        }

        [Test]
        public async Task SellAsset()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result = await marketService.SellAsset(1, "YNDX", 700000, 1,
                stockType.Id, DateTime.Now);
            Assert.IsTrue(result.IsSuccess);

            var stock = marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "YNDX");
            Assert.IsNull(stock);
        }

        [Test]
        public async Task SellAsset__invalidData()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result1 = await marketService.SellAsset(-1, "YNDX", 700000, 1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result1.IsSuccess);

            var result2 = await marketService.SellAsset(1, "YNDX", 700000, 2,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result2.IsSuccess);

            var result3 = await marketService.SellAsset(1, "YNDX", 700000, -1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result3.IsSuccess);

            var result4 = await marketService.SellAsset(1, "YNDX", 700000, 1,
                -1, DateTime.Now);
            Assert.IsFalse(result4.IsSuccess);

            var result5 = await marketService.SellAsset(1, "YNDX", -1, 1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result5.IsSuccess);

            var result6 = await marketService.SellAsset(1, "NONE", 700000, 1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result6.IsSuccess);

            var stock = marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "YNDX");
            Assert.IsNotNull(stock);
        }

        [Test]
        public void GetAllAssetOperations()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var operations = marketService.GetAllAssetOperations(1);

            Assert.AreEqual(4, operations.Count());
            Assert.AreEqual("YNDX", operations.FirstOrDefault().Ticket);
        }

        [Test]
        public void GetAllCost()
        {
            var marketService = new MarketService(_financeDataService, _assetFactory, _balanceService);
            var allCost = marketService.GetAllCost(1);

            Assert.AreEqual(850890, allCost);
        }
    }
}