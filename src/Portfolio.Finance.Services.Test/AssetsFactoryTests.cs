using NUnit.Framework;
using Portfolio.Finance.Services.Services;
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
            var stockMarketData = new StockMarketData(stockMarketAPI);

            TestHelpers.MockStockData(mockHttp);
            TestHelpers.MockDividendData(mockHttp);

            var context = TestHelpers.GetMockFinanceDbContext();
            TestHelpers.SeedOperations1(context);
            var financeDataService = new FinanceDataService(context);

            _assetFactory = new AssetsFactory(financeDataService, stockMarketData);
        }

        [Test]
        public void AssetListCheck()
        {
            var assetList = _assetFactory.Create(1);

            Assert.AreEqual(2, assetList.Count);

            Assert.AreEqual("YNDX", assetList[0].Ticket);
            Assert.AreEqual(624860 - 312430, assetList[0].BoughtPrice);
            Assert.AreEqual(1, assetList[0].Amount);

            Assert.AreEqual("SBER", assetList[1].Ticket);
            Assert.AreEqual(4, assetList[1].Amount);
            Assert.AreEqual(1012430 + 212430, assetList[1].BoughtPrice);
        }
    }
}