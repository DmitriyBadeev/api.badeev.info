using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Test.ServicesTests
{
    [TestFixture]
    public class PortfolioServiceTests
    {
        private PortfolioService _portfolioService;
        private FinanceDataService _financeDataService;
        private BalanceService _balanceService;

        [SetUp]
        public async Task Setup()
        {
            var context = TestHelpers.GetMockFinanceDbContext(); 
            _financeDataService = new FinanceDataService(context);
            _balanceService = new BalanceService(_financeDataService);
            TestHelpers.SeedApp(context);
            _portfolioService = new PortfolioService(_financeDataService, _balanceService);
            
            await MockData();
            await _balanceService.RefillBalance(10, 100000, DateTime.Now);
            await _balanceService.RefillBalance(11, 200000, DateTime.Now);
        }

        [Test]
        public async Task CreateTest()
        {
            var name = "Тестовый портфель 2";
            var result = await _portfolioService.CreatePortfolio(name, 1);

            Assert.IsTrue(result.IsSuccess);

            var containsPortfolio = await 
                _financeDataService.EfContext.Portfolios.AnyAsync(p => p.Name == name && p.UserId == 1);
            
            Assert.IsTrue(containsPortfolio);
        }

        [Test]
        public async Task CreateTest__SameName()
        {
            var name = "Тестовый портфель 2";

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
            var name = "Тестовый портфель 2";
            var result1 = await _portfolioService.CreatePortfolio(name,1);
            Assert.IsTrue(result1.IsSuccess);

            var result2 = await _portfolioService.CreatePortfolio(name, 2);
            Assert.IsTrue(result2.IsSuccess);

            var portfolios = _portfolioService.GetPortfolios(1);

            Assert.AreEqual(3, portfolios.Count());
        }

        [Test]
        public async Task AddPaymentInPortfolio()
        {
            var result1 = await _portfolioService.AddPaymentInPortfolio(10, 1,"SBER", 30, 
                60000, DateTime.Now);
            
            var result2 = await _portfolioService.AddPaymentInPortfolio(999, 1, "SBER", 30, 
                60000, DateTime.Now);
            
            var result3 = await _portfolioService.AddPaymentInPortfolio(10, 2, "SBER", 30, 
                60000, DateTime.Now);
            
            Assert.IsTrue(result1.IsSuccess, "Некорректное добавление выплаты");
            Assert.IsFalse(result2.IsSuccess, "Добавление выплаты в несуществеющий портфель");
            Assert.IsFalse(result3.IsSuccess, "Добавление выплаты в портфель, который не принадлежит пользователю");
        }

        [Test]
        public async Task GetPortfolioPayments()
        {
            var result1 = await _portfolioService.GetPortfolioPayments(10, 1);
            var result2 = await _portfolioService.GetPortfolioPayments(10, 2);
            
            Assert.IsTrue(result1.IsSuccess, "Неуспешное выполнение операции");
            Assert.IsFalse(result2.IsSuccess, "Получение выплат у чужого пользователя");
            Assert.AreEqual(2, result1.Result.Count, "Неверное количество выплат");
        }

        [Test]
        public async Task GetPortfolioPaymentProfit()
        {
            var profit1 = await _portfolioService.GetPortfolioPaymentProfit(10, 1);
            var profit2 = await _portfolioService.GetPortfolioPaymentProfit(11, 1);
            
            var profit3 = await _portfolioService.GetPortfolioPaymentProfit(13, 1);
            var profit4 = await _portfolioService.GetPortfolioPaymentProfit(10, 2);

            Assert.IsTrue(profit1.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(15000, profit1.Result, "Неверная прибыль");
            
            Assert.IsTrue(profit2.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(5000, profit2.Result, "Неверная прибыль");
            
            Assert.IsFalse(profit3.IsSuccess, "Прибыль в несуществующем портфеле");
            Assert.IsFalse(profit4.IsSuccess, "Прибыль у несуществующего пользователя");
        }

        [Test]
        public async Task GetPortfolioPaymentProfitPercent()
        {
            var percent1 = await _portfolioService.GetPortfolioPaymentProfitPercent(10, 1);
            var percent2 = await _portfolioService.GetPortfolioPaymentProfitPercent(11, 1);
            
            Assert.IsTrue(percent1.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(15, percent1.Result, "Неверный процент");
            
            Assert.IsTrue(percent2.IsSuccess, "Неуспешное выполнение операции");
            Assert.AreEqual(2.5, percent2.Result, "Неверный процент");
            
            var percent3 = await _portfolioService.GetPortfolioPaymentProfitPercent(13, 1);
            var percent4 = await _portfolioService.GetPortfolioPaymentProfitPercent(10, 2);
            
            Assert.IsFalse(percent3.IsSuccess, "Несуществующий портфель");
            Assert.IsFalse(percent4.IsSuccess, "Несуществующий пользователь");
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
                }
            };
            
            await _financeDataService.EfContext.Payments.AddRangeAsync(payments);
            await _financeDataService.EfContext.SaveChangesAsync();
        }
    }
}