using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Test.ServicesTests
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

            Assert.AreEqual(762710 - 81840 - 205621, balance1);
            Assert.AreEqual(28570, balance2);
            Assert.AreEqual(28570, balance3);
        }

        [Test]
        public void GetAllBalanceUser()
        {
            var allUserBalance1 = _balanceService.GetAllBalanceUser(1);
            var allUserBalance2 = _balanceService.GetAllBalanceUser(2);

            Assert.AreEqual(762710 + 28570 - 81840 - 205621, allUserBalance1);
            Assert.AreEqual(28570, allUserBalance2);
        }

        [Test]
        public async Task RefillBalance()
        {
            var balanceBefore = _balanceService.GetBalance(1);
            var result = await _balanceService.RefillBalance(1, 300000, DateTime.Now);
            
            Assert.IsTrue(result.IsSuccess);
            var balanceAfter = _balanceService.GetBalance(1);
            Assert.AreEqual(balanceAfter - balanceBefore, 300000);
        }

        [Test]
        public async Task RefillBalance__invalidData()
        {
            var result1 = await _balanceService.RefillBalance(-1, 300000, DateTime.Now);
            var result2 = await _balanceService.RefillBalance(1, -1000, DateTime.Now);

            Assert.IsFalse(result1.IsSuccess);
            Assert.IsFalse(result2.IsSuccess);
        }

        [Test]
        public async Task WithdrawalBalance()
        {
            var balanceBefore = _balanceService.GetBalance(1);
            var result = await _balanceService.WithdrawalBalance(1, 100000, DateTime.Now);

            Assert.IsTrue(result.IsSuccess);
            var balanceAfter = _balanceService.GetBalance(1);
            Assert.AreEqual(balanceBefore - balanceAfter, 100000);
        }

        [Test]
        public async Task WithdrawalBalance__invalidData()
        {
            var result1 = await _balanceService.WithdrawalBalance(-1, 300000, DateTime.Now);
            var result2 = await _balanceService.WithdrawalBalance(1, -1000, DateTime.Now);
            var result3 = await _balanceService.WithdrawalBalance(1, 99999999, DateTime.Now);

            Assert.IsFalse(result1.IsSuccess);
            Assert.IsFalse(result2.IsSuccess);
            Assert.IsFalse(result3.IsSuccess);
        }

        [Test]
        public void GetAllCurrencyOperations()
        {
            var result = _balanceService.GetAllCurrencyOperations(1);
           
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetAllInvestSum()
        {
            var result = _balanceService.GetAllInvestSum(1);

            Assert.AreEqual(2350000, result);
        }
    }
}