﻿using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class MarketServiceTests
    {
        private FinanceDataService _financeDataService;
        private IMarketService _marketService;
        private IPortfolioService _portfolioService;
        
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
            _portfolioService = new PortfolioService(_financeDataService, balanceService, assetFactory);
        }

        [Test]
        public async Task BuyAsset()
        {
            var stockType =
                _financeDataService.EfContext.AssetTypes.FirstOrDefault(t => t.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var result = await _marketService.BuyAsset(1, "MTSS", 2000, 10,
                stockType.Id, DateTime.Now);
            Assert.IsTrue(result.IsSuccess);

            var stock = _portfolioService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "MTSS");
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

            var stock = _portfolioService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "MTSS");
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

            var stock = _portfolioService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "YNDX");
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

            var stock = _portfolioService.GetStocks(1, 1).FirstOrDefault(s => s.Ticket == "YNDX");
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