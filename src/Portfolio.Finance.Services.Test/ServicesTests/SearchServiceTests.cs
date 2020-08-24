using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class SearchServiceTests
    {
        private ISearchService _searchService;
        private MockHttpMessageHandler _mockHttp;

        [SetUp]
        public void Setup()
        {
            _mockHttp = new MockHttpMessageHandler();
            var client = _mockHttp.ToHttpClient();
            var stockMarketAPI = new StockMarketAPI(client);
            MockData();
            var stockMarketData = new StockMarketData(stockMarketAPI);

            var context = TestHelpers.GetMockFinanceDbContext();
            TestHelpers.SeedOperations1(context);
            var financeDataService = new FinanceDataService(context);

            TestHelpers.MockStockData(_mockHttp);
            TestHelpers.MockFondData(_mockHttp);
            TestHelpers.MockBondData(_mockHttp);

            TestHelpers.MockDividendData(_mockHttp);
            TestHelpers.MockCouponsData(_mockHttp);
            
            _searchService = new SearchService(stockMarketData, financeDataService);
        }

        [Test]
        public async Task Search()
        {
            var data = await _searchService.Search("SBER");

            Assert.AreEqual("SBER", data.Ticket);
            Assert.AreEqual("Сбербанк".Length, data.Name.Length);
            Assert.AreEqual("common_share", data.Type);
            Assert.AreEqual("Акция обыкновенная".Length, data.TypeName.Length);
        }

        [Test]
        public async Task Search_InvalidTicker()
        {
            var data = await _searchService.Search("BLABLA");

            Assert.IsNull(data);
        }

        [Test]
        public async Task GetStockData()
        {
            var stockData = await _searchService.GetAssetData("SBER", 1);

            Assert.AreEqual(5, stockData.Amount);
            Assert.AreEqual("SBER", stockData.Ticket);
            Assert.AreEqual("Сбербанк".Length, stockData.Name.Length);
            Assert.AreEqual(226.58, stockData.Price);
            Assert.AreEqual(1132.9, stockData.AllPrice);
            Assert.AreEqual(7, stockData.Payments.Count());
        }

        [Test]
        public async Task GetFondData()
        {
            var fondData = await _searchService.GetAssetData("FXGD", 1);

            Assert.AreEqual(1, fondData.Amount);
            Assert.AreEqual("FXGD", fondData.Ticket);
            Assert.AreEqual("FXGD ETF", fondData.Name);
        }

        [Test]
        public async Task GetBondData()
        {
            var fondData = await _searchService.GetAssetData("SU26209RMFS5", 1);

            Assert.AreEqual(1, fondData.Amount);
            Assert.AreEqual("SU26209RMFS5", fondData.Ticket);
            Assert.AreEqual("ОФЗ 26209".Length, fondData.Name.Length);
        }

        private void MockData()
        {
            var jsonSBER = File.ReadAllTextAsync("TestData/SearchServiceData/SBER_search_response.json").Result;
            var jsonFXGD = File.ReadAllTextAsync("TestData/SearchServiceData/FXGD_search_response.json").Result;
            var jsonOFZ = File.ReadAllTextAsync("TestData/SearchServiceData/SU26209RMFS5_search_response.json").Result;
            var jsonBLABLA = File.ReadAllTextAsync("TestData/SearchServiceData/BLABLA_search_response.json").Result;

            _mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/securities/SBER.json?iss.meta=off&iss.only=description&description.columns=name,value")
                .Respond("application/json", jsonSBER);

            _mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/securities/FXGD.json?iss.meta=off&iss.only=description&description.columns=name,value")
                .Respond("application/json", jsonFXGD);

            _mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/securities/SU26209RMFS5.json?iss.meta=off&iss.only=description&description.columns=name,value")
                .Respond("application/json", jsonOFZ);

            _mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/securities/BLABLA.json?iss.meta=off&iss.only=description&description.columns=name,value")
                .Respond("application/json", jsonBLABLA);
        }
    }
}