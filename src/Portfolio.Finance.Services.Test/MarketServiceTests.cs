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
        private FinanceDataService _financeDataService;
        private IMarketService _marketService;

        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            var stockMarketData = new StockMarketData(stockMarketAPI);

            TestHelpers.MockStockData(mockHttp);
            TestHelpers.MockFondData(mockHttp);
            TestHelpers.MockBondData(mockHttp);

            TestHelpers.MockDividendData(mockHttp);
            TestHelpers.MockCouponsData(mockHttp);

            var context = TestHelpers.GetMockFinanceDbContext();
            TestHelpers.SeedOperations1(context);

            _financeDataService = new FinanceDataService(context);
            var assetFactory = new AssetsFactory(_financeDataService, stockMarketData);
            var balanceService = new BalanceService(_financeDataService);

            _marketService = new MarketService(_financeDataService, assetFactory, balanceService);
        }

        [Test]
        public void GetAllPaperProfit()
        {
            var profit = _marketService.GetAllPaperProfit(1);
            Assert.AreEqual(-1010710 + 20000 - 99338, profit);
        }

        [Test]
        public void GetAllProfitTest__otherUser()
        {
            var profit = _marketService.GetAllPaperProfit(2);
            Assert.AreEqual(22658 - 21430, profit);
        }

        [Test]
        public void GetAllPaperPrice()
        {
            var price = _marketService.GetAllPaperPrice(1);
            Assert.AreEqual(548010 + 101840 + 106283, price);
        }

        [Test]
        public void GetAllPaymentProfit()
        {
            var profit = _marketService.GetAllPaymentProfit(1);

            Assert.AreEqual(11600 + 117354, profit);
        }

        [Test]
        public void GetStocks()
        {
            var stocks1 = _marketService.GetStocks(1, 1);
            var stocks2 = _marketService.GetStocks(1, 2);

            var stocksInvalid = _marketService.GetStocks(1, 10);

            Assert.AreEqual(2, stocks1.Count());
            Assert.AreEqual(1, stocks2.Count());

            Assert.AreEqual(0, stocksInvalid.Count());
        }

        [Test]
        public void GetFonds()
        {
            var fonds1 = _marketService.GetFonds(1, 1);
            var fonds2 = _marketService.GetFonds(1, 2);

            var fondsInvalid = _marketService.GetFonds(1, 10);

            Assert.AreEqual(1, fonds1.Count());
            Assert.AreEqual(0, fonds2.Count());

            Assert.AreEqual(0, fondsInvalid.Count());
        }

        [Test]
        public void GetBonds()
        {
            var bonds1 = _marketService.GetBonds(1, 1);
            var bonds2 = _marketService.GetBonds(1, 2);

            var bondsInvalid = _marketService.GetFonds(1, 10);

            Assert.AreEqual(2, bonds1.Count());
            Assert.AreEqual(0, bonds2.Count());

            Assert.AreEqual(0, bondsInvalid.Count());
        }

        [Test]
        public async Task BuyAsset()
        {
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result = await _marketService.BuyAsset(1, "MTSS", 2000, 10,
                stockType.Id, DateTime.Now);
            Assert.IsTrue(result.IsSuccess);

            var stock = _marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "MTSS");
            Assert.IsNotNull(stock);
        }

        [Test]
        public async Task BuyAsset__invalidData()
        {
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result1 = await _marketService.BuyAsset(-1, "MTSS", 2000, 10,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result1.IsSuccess);

            var result2 = await _marketService.BuyAsset(1, "MTSS", 99999999, 10,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result2.IsSuccess);

            var result3 = await _marketService.BuyAsset(1, "MTSS", 2000, -1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result3.IsSuccess);

            var result4 = await _marketService.BuyAsset(1, "MTSS", 2000, 10,
                -1, DateTime.Now);
            Assert.IsFalse(result4.IsSuccess);

            var result5 = await _marketService.BuyAsset(1, "MTSS", -1, 10,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result5.IsSuccess);

            var stock = _marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "MTSS");
            Assert.IsNull(stock);
        }

        [Test]
        public async Task SellAsset()
        {
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result = await _marketService.SellAsset(1, "YNDX", 700000, 1,
                stockType.Id, DateTime.Now);
            Assert.IsTrue(result.IsSuccess);

            var stock = _marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "YNDX");
            Assert.IsNull(stock);
        }

        [Test]
        public async Task SellAsset__invalidData()
        {
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result1 = await _marketService.SellAsset(-1, "YNDX", 700000, 1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result1.IsSuccess);

            var result2 = await _marketService.SellAsset(1, "YNDX", 700000, 2,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result2.IsSuccess);

            var result3 = await _marketService.SellAsset(1, "YNDX", 700000, -1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result3.IsSuccess);

            var result4 = await _marketService.SellAsset(1, "YNDX", 700000, 1,
                -1, DateTime.Now);
            Assert.IsFalse(result4.IsSuccess);

            var result5 = await _marketService.SellAsset(1, "YNDX", -1, 1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result5.IsSuccess);

            var result6 = await _marketService.SellAsset(1, "NONE", 700000, 1,
                stockType.Id, DateTime.Now);
            Assert.IsFalse(result6.IsSuccess);

            var stock = _marketService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "YNDX");
            Assert.IsNotNull(stock);
        }

        [Test]
        public void GetAllAssetOperations()
        {
            var operations = _marketService.GetAllAssetOperations(1);

            Assert.AreEqual(7, operations.Count());
            Assert.AreEqual("YNDX", operations.FirstOrDefault().Ticket);
        }

        [Test]
        public void GetAllCost()
        {
            var allCost = _marketService.GetAllCost(1);

            Assert.AreEqual(756133 + 128954 + 503819, allCost);
        }

        [Test]
        public void GetPercentPaperProfit()
        {
            var percent = _marketService.GetPercentOfPaperProfit(1);

            Assert.AreEqual(-46.4, percent);
        }

        [Test]
        public void GetPercentOfPaymentProfit()
        {
            var percent = _marketService.GetPercentOfPaymentProfit(1);

            Assert.AreEqual(5.5, percent);
        }

        [Test]
        public void GetUserBalanceWithPaidPayments()
        {
            var balance = _marketService.GetUserBalanceWithPaidPayments(1);

            Assert.AreEqual(503819 + 128954, balance);
        }

        [Test]
        public async Task GetAllAssetPrices__CommonPath()
        {
            var assetPrices = await _marketService.GetAllAssetPrices(1);
            
            Assert.AreEqual(548010, assetPrices.StockPrice);
            Assert.AreEqual(101840, assetPrices.FondPrice);
            Assert.AreEqual(106283, assetPrices.BondPrice);
        }

        [Test]
        public async Task GetAllAssetPrices__WithZeroSumPrices()
        {
            var assetPricesUser2 = await _marketService.GetAllAssetPrices(2);

            Assert.AreEqual(22658, assetPricesUser2.StockPrice);
            Assert.AreEqual(0, assetPricesUser2.FondPrice);
            Assert.AreEqual(0, assetPricesUser2.BondPrice);
        }

        [Test]
        public async Task GetAllAssetPrices__WrongUser()
        {
            var assetPricesUser3 = await _marketService.GetAllAssetPrices(3);

            Assert.AreEqual(0, assetPricesUser3.StockPrice);
            Assert.AreEqual(0, assetPricesUser3.FondPrice);
            Assert.AreEqual(0, assetPricesUser3.BondPrice);
        }

        [Test]
        public void GetAllFuturePayments()
        {
            var payments = _marketService.GetAllFuturePayments(1);

            Assert.AreEqual(7, payments.Count);
        }

        [Test]
        public void GetAllFuturePayments__otherUser()
        {
            var payments = _marketService.GetAllFuturePayments(2);

            Assert.AreEqual(1, payments.Count);
        }
    }
}