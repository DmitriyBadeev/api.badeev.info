using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class AggregatePortfolioServiceTests
    {
        private IAggregatePortfolioService _aggregatePortfolioService;
        private FinanceDataService _financeDataService;
        
        [SetUp]
        public async Task Setup()
        {
            var context = TestHelpers.GetMockFinanceDbContext(); 
            _financeDataService = new FinanceDataService(context);
            var balanceService = new BalanceService(_financeDataService);
            TestHelpers.SeedApp(context);
            var portfolioService = new PortfolioService(_financeDataService, balanceService);
            _aggregatePortfolioService = new AggregatePortfolioService(portfolioService, balanceService);
            
            await MockData();
            await balanceService.RefillBalance(10, 100000, DateTime.Now);
            await balanceService.RefillBalance(11, 200000, DateTime.Now);
            await balanceService.RefillBalance(12, 100000, DateTime.Now);
        }

        [Test]
        public async Task AggregatePayments()
        {
            var result1 = await _aggregatePortfolioService.AggregatePayments(new[] {10, 11}, 1);
            var result2 = await _aggregatePortfolioService.AggregatePayments(new[] {10, 11, 12}, 1);
            var result3 = await _aggregatePortfolioService.AggregatePayments(new[] {10}, 1);
            var result4 = await _aggregatePortfolioService.AggregatePayments(new[] {12}, 2);
            var result5 = await _aggregatePortfolioService.AggregatePayments(new[] {12}, 1);
            
            Assert.IsTrue(result1.IsSuccess);
            Assert.AreEqual(3, result1.Result.Count);
            
            Assert.IsTrue(result3.IsSuccess);
            Assert.AreEqual(2, result3.Result.Count);

            Assert.IsTrue(result4.IsSuccess);
            Assert.AreEqual(1, result4.Result.Count);
            
            Assert.IsFalse(result2.IsSuccess, "Считается портфель чужого пользователя");
            Assert.IsFalse(result5.IsSuccess, "Считается портфель чужого пользователя");
        }

        [Test]
        public async Task AggregatePaymentProfit()
        {
            var result1 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {10, 11}, 1);
            var result2 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {10, 11, 12}, 1);
            var result3 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {10}, 1);
            var result4 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {12}, 2);
            var result5 = await _aggregatePortfolioService.AggregatePaymentProfit(new[] {12}, 1);
            
            Assert.IsTrue(result1.IsSuccess);
            Assert.AreEqual(20000, result1.Result.Value);
            Assert.AreEqual(6.7, result1.Result.Percent);
            
            Assert.IsTrue(result3.IsSuccess);
            Assert.AreEqual(15000, result3.Result.Value);
            Assert.AreEqual(15, result3.Result.Percent);
            
            Assert.IsTrue(result4.IsSuccess);
            Assert.AreEqual(10000, result4.Result.Value);
            Assert.AreEqual(10, result4.Result.Percent);
            
            Assert.IsFalse(result2.IsSuccess, "Считается портфель чужого пользователя");
            Assert.IsFalse(result5.IsSuccess, "Считается портфель чужого пользователя");
        }

        private async Task MockData()
        {
            var portfolios = new List<Core.Entities.Finance.Portfolio>()
            {
                new Core.Entities.Finance.Portfolio()
                {
                    Id = 10,
                    Name = "Тестовый портфель",
                    UserId = 1,
                },
                new Core.Entities.Finance.Portfolio()
                {
                    Id = 11,
                    Name = "Другой тестовый портфель",
                    UserId = 1,
                },
                new Core.Entities.Finance.Portfolio()
                {
                    Id = 12,
                    Name = "Тестовый портфель другого пользователя",
                    UserId = 2,
                },
            };
            
            await _financeDataService.EfContext.Portfolios.AddRangeAsync(portfolios);
            await _financeDataService.EfContext.SaveChangesAsync();

            var payments = new List<Payment>()
            {
                new Payment()
                {
                    Id = 10,
                    PortfolioId = 10,
                    Ticket = "SBER",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 10000
                },
                new Payment()
                {
                    Id = 11,
                    PortfolioId = 10,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 5000
                },
                new Payment()
                {
                    Id = 12,
                    PortfolioId = 11,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 5000
                },
                new Payment()
                {
                    Id = 13,
                    PortfolioId = 12,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 10000
                }
            };
            
            await _financeDataService.EfContext.Payments.AddRangeAsync(payments);
            await _financeDataService.EfContext.SaveChangesAsync();
        }
    }
}