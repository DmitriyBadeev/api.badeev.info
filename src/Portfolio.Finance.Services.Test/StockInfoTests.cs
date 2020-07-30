using System.IO;
using System.Net.Http;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class StockInfoTests
    {
        private IStockMarketData _stockMarketData;
        private MockHttpMessageHandler _mockHttp;

        [SetUp]
        public void Setup()
        { 
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            _stockMarketData = new Services.StockMarketData(stockMarketAPI);
        }

        [Test]
        public void GetPrice1()
        {
            var jsonYNDX = File.ReadAllTextAsync("TestData/stock_response_YNDX.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", jsonYNDX);

            var stockInfo = new StockInfo(_stockMarketData, "YNDX", 1, 342150);
            var price = stockInfo.GetPrice();

            Assert.AreEqual(4078, price);
        }

        [Test]
        public void GetPrice2()
        {
            var jsonSBER = File.ReadAllTextAsync("TestData/stock_response_SBER.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/SBER.json")
                .Respond("application/json", jsonSBER);

            var stockInfo = new StockInfo(_stockMarketData, "SBER", 1, 20779);
            var price = stockInfo.GetPrice();

            Assert.AreEqual(217.79, price);
        }

        [Test]
        public void GetPaperProfit1()
        {
            var jsonYNDX = File.ReadAllTextAsync("TestData/stock_response_YNDX.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", jsonYNDX);

            var stockInfo = new StockInfo(_stockMarketData, "YNDX", 1, 307279);
            var profit = stockInfo.GetPaperProfit();

            Assert.AreEqual(4078 - 3072.79, profit);
        }

        [Test]
        public void GetPaperProfit2()
        {
            var jsonSBER = File.ReadAllTextAsync("TestData/stock_response_SBER.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/SBER.json")
                .Respond("application/json", jsonSBER);

            var stockInfo = new StockInfo(_stockMarketData, "SBER", 1, 20779);
            var profit = stockInfo.GetPaperProfit();

            Assert.AreEqual(217.79 - 207.79, profit);
        }
    }
}