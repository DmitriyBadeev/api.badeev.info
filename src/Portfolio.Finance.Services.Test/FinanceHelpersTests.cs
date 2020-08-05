using System.Text.Json;
using NUnit.Framework;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class FinanceHelpersTests
    {
        private AssetResponse _data;

        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            var stockMarketData = new StockMarketData(stockMarketAPI);

            TestHelpers.MockStockData(mockHttp);

            _data = stockMarketData.GetStockData("YNDX").Result;
        }

        [Test]
        public void GetPriceDouble()
        {
            var doublePrice = FinanceHelpers.GetPriceDouble(302152);

            Assert.AreEqual(3021.52, doublePrice);
        }

        [Test]
        public void GetPriceDouble__wholeNumber()
        {
            var doublePrice = FinanceHelpers.GetPriceDouble(30215200);

            Assert.AreEqual(302152, doublePrice);
        }

        [Test]
        public void GetValueOfColumnMarketdata()
        {
            var strPrice = FinanceHelpers.GetValueOfColumnMarketdata("LAST", _data);

            Assert.AreEqual(434720, (int)(strPrice.GetDouble() * 100));
        }

        [Test]
        public void GetValueOfColumnSecurities()
        {
            var shortName = FinanceHelpers.GetValueOfColumnSecurities("SHORTNAME", _data);

            Assert.AreEqual("Yandex clA", shortName.GetString());
        }

        [Test]
        public void GetValueOfColumn__invalidIndex()
        {
            var strPrice = FinanceHelpers.GetValueOfColumnMarketdata("BLABLA", _data);

            Assert.AreEqual(JsonValueKind.Undefined, strPrice.ValueKind);
        }
    }
}