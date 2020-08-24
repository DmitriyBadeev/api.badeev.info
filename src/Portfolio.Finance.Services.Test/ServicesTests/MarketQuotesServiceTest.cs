using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class MarketQuotesServiceTest
    {
        private IMarketQuotesService _marketQuotesService;
        private MockHttpMessageHandler _mockHttp;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();
            var stockMarketAPI = new StockMarketAPI(client);
            MockData();
            var stockMarketData = new StockMarketData(stockMarketAPI);
            _marketQuotesService = new MarketQuotesService(stockMarketData);
        }

        [Test]
        public void GetMarketQuotes()
        {
            var quotes = _marketQuotesService.GetMarketQuotes();

            Assert.AreEqual(5, quotes.Count());
        }

        [Test]
        public async Task GetIMOEX()
        {
            var index = await _marketQuotesService.GetIMOEX();

            Assert.AreEqual("Индекс МосБиржи".Length, index.Name.Length);
            Assert.AreEqual("IMOEX", index.Ticket);
            Assert.AreEqual("18:40:00", index.Time);
            Assert.AreEqual(3052.46, index.Value);
            Assert.AreEqual(-0.15, index.Change);
        }

        [Test]
        public async Task GetRTSI()
        {
            var index = await _marketQuotesService.GetRTSI();

            Assert.AreEqual("Индекс РТС".Length, index.Name.Length);
            Assert.AreEqual("RTSI", index.Ticket);
            Assert.AreEqual("18:40:00", index.Time);
            Assert.AreEqual(1308.71, index.Value);
            Assert.AreEqual(0.51, index.Change);
        }

        [Test]
        public async Task GetUSD()
        {
            var currency = await _marketQuotesService.GetUSD();

            Assert.AreEqual("USD", currency.Name);
            Assert.AreEqual("USD000UTSTOM", currency.Ticket);
            Assert.AreEqual("19:35:15", currency.Time);
            Assert.AreEqual(73.28, currency.Value);
            Assert.AreEqual(-0.46, currency.Change);
        }

        [Test]
        public async Task GetEURO()
        {
            var currency = await _marketQuotesService.GetEURO();

            Assert.AreEqual("EURO", currency.Name);
            Assert.AreEqual("EUR_RUB__TOM", currency.Ticket);
            Assert.AreEqual("19:35:42", currency.Time);
            Assert.AreEqual(87.46, currency.Value);
            Assert.AreEqual(-0.04, currency.Change);
        }

        [Test]
        public async Task GetBrent()
        {
            var brent = await _marketQuotesService.GetBrent();

            Assert.AreEqual("Нефть", brent.Name);
            Assert.AreEqual("BRU0", brent.Ticket);
            Assert.AreEqual("20:37:54", brent.Time);
            Assert.AreEqual(45.34, brent.Value);
            Assert.AreEqual(-0.11, brent.Change);
        }

        private void MockData()
        {
            var jsonIMOEX = File.ReadAllTextAsync("TestData/QuotesData/IMOEX_response.json").Result;
            var jsonRTSI = File.ReadAllTextAsync("TestData/QuotesData/RTSI_response.json").Result;

            var jsonUSD = File.ReadAllTextAsync("TestData/QuotesData/USD_response.json").Result;
            var jsonEURO = File.ReadAllTextAsync("TestData/QuotesData/EURO_response.json").Result;

            var jsonBrent = File.ReadAllTextAsync("TestData/QuotesData/BRENT_response.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/index/securities/IMOEX.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonIMOEX);

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/index/securities/RTSI.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonRTSI);

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/currency/markets/selt/boards/CETS/securities/USD000UTSTOM.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonUSD);

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/currency/markets/selt/boards/CETS/securities/EUR_RUB__TOM.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonEURO);

            _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/futures/markets/forts/securities/BRU0.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonBrent);
        }
    }
}