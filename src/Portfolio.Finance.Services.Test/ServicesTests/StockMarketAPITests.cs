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
    public class StockMarketAPITests
    {
        private MockHttpMessageHandler _mockHttp;
        private IStockMarketAPI _stockMarketAPI;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();

            _stockMarketAPI = new StockMarketAPI(client);
        }

        [Test]
        public async Task MakeRequest()
        {
            var request = _mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/YNDX.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", "YNDX");

            var response = await _stockMarketAPI.FindStock("YNDX");

            Assert.AreEqual("YNDX", response.JsonContent);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(1, _mockHttp.GetMatchCount(request));
        }
    }
}