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
        private StockResponse _data;

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
        public void GetValueOfColumn()
        {
            var stockInfoList = FinanceHelpers.GetStockInfo("TQBR", _data);
            var strPrice = FinanceHelpers.GetValueOfColumn("LAST", stockInfoList, _data);

            Assert.AreEqual(4078, strPrice.GetDouble());
        }

        [Test]
        public void GetValueOfColumn__invalidIndex()
        {
            var stockInfoList = FinanceHelpers.GetStockInfo("TQBR", _data);
            var strPrice = FinanceHelpers.GetValueOfColumn("BLABLA", stockInfoList, _data);

            Assert.AreEqual(JsonValueKind.Undefined, strPrice.ValueKind);
        }
    }
}