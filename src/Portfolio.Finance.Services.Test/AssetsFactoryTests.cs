using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class AssetsFactoryTests
    {
        private AssetsFactory _assetFactory;

        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            var stockMarketData = new Services.StockMarketData(stockMarketAPI);

            var jsonYNDX = File.ReadAllTextAsync("TestData/stock_response_YNDX.json").Result;
            var jsonSBER = File.ReadAllTextAsync("TestData/stock_response_SBER.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", jsonYNDX);

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/SBER.json")
                .Respond("application/json", jsonSBER);

            var context = TestHelpers.GetMockFinanceDbContext();
            SeedOperations(context);
            var financeDataService = new FinanceDataService(context);

            _assetFactory = new AssetsFactory(financeDataService, stockMarketData);
        }

        [Test]
        public void AssetListCheck()
        {
            var assetList = _assetFactory.Create();

            Assert.AreEqual(2, assetList.Count);

            Assert.AreEqual("YNDX", assetList[0].Ticket);
            Assert.AreEqual(624860 - 312430, assetList[0].BoughtPrice);
            Assert.AreEqual(1, assetList[0].Amount);

            Assert.AreEqual("SBER", assetList[1].Ticket);
            Assert.AreEqual(4, assetList[1].Amount);
            Assert.AreEqual(1012430 + 212430, assetList[1].BoughtPrice);
        }

        private void SeedOperations(FinanceDbContext context)
        {
            var mockLogger = new Mock<ILogger<SeedFinanceDataService>>();
            var seedService = new SeedFinanceDataService(mockLogger.Object, context);

            seedService.Initialise();
            var buyAction = context.AssetActions.FirstOrDefault(a => a.Name == SeedFinanceData.BUY_ACTION);
            var sellAction = context.AssetActions.FirstOrDefault(a => a.Name == SeedFinanceData.SELL_ACTION);
            var stockType = context.AssetTypes.FirstOrDefault(a => a.Name == SeedFinanceData.STOCK_ASSET_TYPE);

            var operations = new List<AssetOperation>()
            {
                new AssetOperation()
                {
                    Id = 1,
                    Ticket = "YNDX",
                    Amount = 2,
                    PaymentPrice = 624860,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id
                },
                new AssetOperation()
                {
                    Id = 2,
                    Ticket = "YNDX",
                    Amount = 1,
                    PaymentPrice = 312430,
                    AssetAction = sellAction,
                    AssetActionId = sellAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id
                },
                new AssetOperation()
                {
                    Id = 3,
                    Ticket = "SBER",
                    Amount = 3,
                    PaymentPrice = 1012430,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id
                },
                new AssetOperation()
                {
                    Id = 4,
                    Ticket = "SBER",
                    Amount = 1,
                    PaymentPrice = 212430,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id
                }
            };

            context.AssetOperations.AddRange(operations);
            context.SaveChanges();
        }
    }
}