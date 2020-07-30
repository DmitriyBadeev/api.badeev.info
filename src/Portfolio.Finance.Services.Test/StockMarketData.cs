using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class StockMarketData
    {
        private MockHttpMessageHandler _mockHttp;
        private IStockMarketData _stockMarketData;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            _stockMarketData = new Services.StockMarketData(stockMarketAPI);
        }

        [Test]
        public async Task GetValidStockData1()
        {
            var json = await File.ReadAllTextAsync("TestData/stock_response_YNDX.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetStockData("YNDX");

            Assert.AreEqual("YNDX", response.marketdata.data[0][0].ToString());
            Assert.AreEqual(56, response.marketdata.columns.Count);
        }

        [Test]
        public async Task GetValidStockData2()
        {
            var json = await File.ReadAllTextAsync("TestData/stock_response_SBER.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/SBER.json")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetStockData("SBER");

            Assert.AreEqual("SBER", response.marketdata.data[0][0].ToString());
            Assert.AreEqual(56, response.marketdata.columns.Count);
        }

        [Test]
        public async Task HandleError()
        {
            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond(HttpStatusCode.BadGateway);

            var response = await _stockMarketData.GetStockData("YNDX");

            Assert.AreEqual(null, response);
        }
    }
}