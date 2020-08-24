using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class PortfolioServiceTests
    {
        private PortfolioService _portfolioService;
        private FinanceDataService _financeDataService;

        [SetUp]
        public void Setup()
        {
            var context = TestHelpers.GetMockFinanceDbContext(); 
            _financeDataService = new FinanceDataService(context);
            _portfolioService = new PortfolioService(_financeDataService);
        }

        [Test]
        public async Task CreateTest()
        {
            var name = "Тестовый портфель";
            var result = await _portfolioService.CreatePortfolio(name, 1);

            Assert.IsTrue(result.IsSuccess);

            var containsPortfolio = await 
                _financeDataService.EfContext.Portfolios.AnyAsync(p => p.Name == name && p.UserId == 1);
            
            Assert.IsTrue(containsPortfolio);
        }

        [Test]
        public async Task CreateTest__SameName()
        {
            var name = "Тестовый портфель";

            var result1 = await _portfolioService.CreatePortfolio(name, 1);
            Assert.IsTrue(result1.IsSuccess);

            var result2 = await _portfolioService.CreatePortfolio(name, 1);
            Assert.IsFalse(result2.IsSuccess);

            var result3 = await _portfolioService.CreatePortfolio(name, 2);
            Assert.IsTrue(result3.IsSuccess);
        }

        [Test]
        public async Task GetPortfoliosTest()
        {
            var name = "Тестовый портфель";
            var result1 = await _portfolioService.CreatePortfolio(name,1);
            Assert.IsTrue(result1.IsSuccess);

            var result2 = await _portfolioService.CreatePortfolio(name, 2);
            Assert.IsTrue(result2.IsSuccess);

            var portfolios = _portfolioService.GetPortfolios(1);

            Assert.AreEqual(1, portfolios.Count());
        }
    }
}