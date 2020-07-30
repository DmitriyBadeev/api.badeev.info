using System.IO;
using System.Net.Http;
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
            var stockMarketData = new Services.StockMarketData(stockMarketAPI);

            var json = File.ReadAllTextAsync("TestData/stock_response.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", json);

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
            var strPrice = FinanceHelpers.GetValueOfColumn("LAST", _data);

            Assert.AreEqual("4078", strPrice);
        }

        [Test]
        public void GetValueOfColumn__invalidIndex()
        {
            var strPrice = FinanceHelpers.GetValueOfColumn("BLABLA", _data);

            Assert.IsNull(strPrice);
        }
    }
}