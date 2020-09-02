using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class GraphServiceTests
    {
        private IGraphService _graphService;
        private MockHttpMessageHandler _mockHttp;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();
            var stockMarketAPI = new StockMarketAPI(client);
            MockData();
            var stockMarketData = new StockMarketData(stockMarketAPI);
            _graphService = new GraphService(stockMarketData);
        }

        [Test]
        public async Task StockCandles()
        {
            var candles = await _graphService.StockCandles("YNDX", new DateTime(2020, 6, 2), CandleInterval.Day);

            Assert.AreEqual(64, candles.Count);
        }

        private void MockData()
        {
            TestHelpers.MockCandles(_mockHttp);
        }
    }
}