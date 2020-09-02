using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class StockMarketDataTests
    {
        private MockHttpMessageHandler _mockHttp;
        private IStockMarketData _stockMarketData;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();

            var stockMarketAPI = new StockMarketAPI(client);
            _stockMarketData = new StockMarketData(stockMarketAPI);
        }

        [Test]
        public async Task GetValidStockData1()
        {
            var json = await File.ReadAllTextAsync("TestData/AssetsData/stock_response_YNDX.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/YNDX.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetStockData("YNDX");

            Assert.AreEqual("YNDX", response.marketdata.data[0][0].ToString());
            Assert.AreEqual(56, response.marketdata.columns.Count);
        }

        [Test]
        public async Task GetValidStockData2()
        {
            var json = await File.ReadAllTextAsync("TestData/AssetsData/stock_response_SBER.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/SBER.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetStockData("SBER");

            Assert.AreEqual("SBER", response.marketdata.data[0][0].ToString());
            Assert.AreEqual(56, response.marketdata.columns.Count);
        }

        [Test]
        public async Task GetValidFondData()
        {
            var json = await File.ReadAllTextAsync("TestData/AssetsData/fond_response_FXGD.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQTF/securities/FXGD.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetFondData("FXGD");
            Assert.AreEqual("FXGD", response.marketdata.data[0][0].ToString());
        }

        [Test]
        public async Task GetValidBondData()
        {
            var json = await File.ReadAllTextAsync("TestData/AssetsData/bond_response_SU26209RMFS5.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/bonds/boards/TQOB/securities/SU26209RMFS5.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetBondData("SU26209RMFS5");
            Assert.AreEqual("SU26209RMFS5", response.marketdata.data[0][0].ToString());
        }

        [Test]
        public async Task GetDividends()
        {
            var json = await File.ReadAllTextAsync("TestData/DividendsData/dividends_response_SBER.json");

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/securities/SBER/dividends.json?iss.meta=off")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetDividendsData("SBER");

            Assert.AreEqual(5, response.dividends.columns.Count);
            Assert.AreEqual(7, response.dividends.data.Count);
        }

        [Test]
        public async Task GetCouponsData()
        {
            var json = await File.ReadAllTextAsync("TestData/CouponsData/coupons_response_RU000A0JSMA2.json");

            _mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/statistics/engines/stock/markets/bonds/bondization/SU26209RMFS5.json?from=2020-02-07&iss.only=coupons,amortizations&iss.meta=off")
                .Respond("application/json", json);

            var response = await _stockMarketData.GetCouponsData("SU26209RMFS5", new DateTime(2020, 2, 7));

            Assert.AreEqual(12, response.coupons.columns.Count);
            Assert.AreEqual(5, response.coupons.data.Count);
        }

        [Test]
        public async Task GetStockCandleData()
        {
            TestHelpers.MockCandles(_mockHttp);

            var response = await _stockMarketData.GetStockCandleData("YNDX", new DateTime(2020, 6, 2), CandleInterval.Day);

            Assert.AreEqual(64, response.candles.data.Count);
        }

        [Test]
        public async Task HandleErrorStock()
        {
            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/YNDX.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond(HttpStatusCode.BadGateway);

            var response = await _stockMarketData.GetStockData("YNDX");

            Assert.AreEqual(null, response);
        }

        [Test]
        public async Task HandleErrorDividend()
        {
            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/securities/YNDX/dividends.json?iss.meta=off")
                .Respond(HttpStatusCode.BadGateway);

            var response = await _stockMarketData.GetDividendsData("YNDX");

            Assert.AreEqual(null, response);
        }
    }
}