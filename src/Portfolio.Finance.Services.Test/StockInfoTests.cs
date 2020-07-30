using System;
using System.IO;
using System.Net.Http;
using NUnit.Framework;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class StockInfoTests
    {
        private MockHttpMessageHandler _mockHttp;
        private StockInfo _stockInfo;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            var stockMarketData = new Services.StockMarketData(stockMarketAPI);

            var json = File.ReadAllTextAsync("TestData/stock_response.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", json);

            _stockInfo = new StockInfo(stockMarketData, "YNDX", 1, 342150, DateTime.Now);
        }

        [Test]
        public void GetPrice()
        {
            var price = _stockInfo.GetPrice();

            Assert.AreEqual(4078, price);
        }

        [Test]
        public void GetPaperProfit()
        {
            var price = _stockInfo.GetPaperProfit();

            Assert.AreEqual(4078 - 3421.50, price);
        }
    }
}