using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class BalanceServiceTests
    {
        private IBalanceService _balanceService;

        [SetUp]
        public void Setup()
        {
            var context = TestHelpers.GetMockFinanceDbContext();
            TestHelpers.SeedOperations1(context);
            var financeDataService = new FinanceDataService(context);
            _balanceService = new BalanceService(financeDataService);
        }

        [Test]
        public void GetBalance()
        {
            var balance1 = _balanceService.GetBalance(1);
            var balance2 = _balanceService.GetBalance(2);
            var balance3 = _balanceService.GetBalance(3);

            Assert.AreEqual(262710, balance1);
            Assert.AreEqual(28570, balance2);
            Assert.AreEqual(28570, balance3);
        }

        [Test]
        public void GetAllBalanceUser()
        {
            var allUserBalance1 = _balanceService.GetAllBalanceUser(1);
            var allUserBalance2 = _balanceService.GetAllBalanceUser(2);

            Assert.AreEqual(262710 + 28570, allUserBalance1);
            Assert.AreEqual(28570, allUserBalance2);
        }
    }
}