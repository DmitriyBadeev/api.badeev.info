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
        public async Task GetBalance()
        {
            var balance1 = await _balanceService.GetBalance(1, 1);
            var balance2 = await _balanceService.GetBalance(2, 1);
            var balance3 = await _balanceService.GetBalance(3, 2);
            var balance4 = await _balanceService.GetBalance(3, 1);
            
            Assert.IsTrue(balance1.IsSuccess);
            Assert.AreEqual(762710 - 81840 - 205621 + 20000, balance1.Result);
            Assert.AreEqual(28570 + 5000, balance2.Result);
            Assert.AreEqual(28570 + 10000, balance3.Result);
            
            Assert.IsFalse(balance4.IsSuccess);
        }

        [Test]
        public async Task AggregateBalance()
        {
            var balance1 = await _balanceService.AggregateBalance(new [] {1, 2}, 1);
            var balance2 = await _balanceService.AggregateBalance(new [] {1, 2, 3}, 1);
            var balance3 = await _balanceService.AggregateBalance(new [] {3}, 2);
            
            Assert.IsTrue(balance1.IsSuccess);
            Assert.AreEqual(762710 + 28570 - 81840 - 205621 + 25000, balance1.Result);
            
            Assert.IsFalse(balance2.IsSuccess);            
            
            Assert.IsTrue(balance3.IsSuccess);
            Assert.AreEqual(28570 + 10000, balance3.Result);
        }

        [Test]
        public async Task RefillBalance()
        {
            var balanceBefore = await _balanceService.GetBalance(1, 1);
            var result = await _balanceService.RefillBalance(1, 300000, DateTime.Now);
            
            Assert.IsTrue(result.IsSuccess);
            var balanceAfter = await _balanceService.GetBalance(1, 1);
            Assert.AreEqual(balanceAfter.Result - balanceBefore.Result, 300000);
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
            var balanceBefore = await _balanceService.GetBalance(1,1);
            var result = await _balanceService.WithdrawalBalance(1, 100000, DateTime.Now);

            Assert.IsTrue(result.IsSuccess);
            var balanceAfter = await _balanceService.GetBalance(1, 1);
            Assert.AreEqual(balanceBefore.Result - balanceAfter.Result, 100000);
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
        public void GetInvestSum()
        {
            var sum1 = _balanceService.GetInvestSum(1, 1);
            var sum2 = _balanceService.GetInvestSum(2, 1);
            var sum3 = _balanceService.GetInvestSum(3, 2);
            
            var sum4 = _balanceService.GetInvestSum(3, 3);
            
            Assert.AreEqual(2300000, sum1);
            Assert.AreEqual(50000, sum2);
            Assert.AreEqual(50000, sum3);
            
            Assert.AreEqual(0, sum4);
        }

        [Test]
        public void GetAggregateInvestSum()
        {
            var sum1 = _balanceService.GetAggregateInvestSum(new [] {1, 2}, 1);
            var sum2 = _balanceService.GetAggregateInvestSum(new [] {1, 2, 3}, 1);
            var sum3 = _balanceService.GetAggregateInvestSum(new [] {3}, 2);
            var sum4 = _balanceService.GetInvestSum(3, 3);
            
            Assert.AreEqual(2350000, sum1);
            Assert.AreEqual(2350000, sum2);
            Assert.AreEqual(50000, sum3);
            Assert.AreEqual(0, sum4);
        }
    }
}