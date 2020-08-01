using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Portfolio.Core.Entities.Finance;
using Portfolio.Infrastructure;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    public static class TestHelpers
    {
        public static FinanceDbContext GetMockFinanceDbContext()
        {
            var options = new DbContextOptionsBuilder<FinanceDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new FinanceDbContext(options);
        }

        public static void SeedOperations1(FinanceDbContext context)
        {
            var mockLogger = new Mock<ILogger<SeedFinanceDataService>>();
            var seedService = new SeedFinanceDataService(mockLogger.Object, context);

            seedService.Initialise();
            var buyAction = context.AssetActions.FirstOrDefault(a => a.Name == SeedFinanceData.BUY_ACTION);
            var sellAction = context.AssetActions.FirstOrDefault(a => a.Name == SeedFinanceData.SELL_ACTION);
            var stockType = context.AssetTypes.FirstOrDefault(a => a.Name == SeedFinanceData.STOCK_ASSET_TYPE);
            var refillAction = context.CurrencyActions.FirstOrDefault(a => a.Name == SeedFinanceData.REFILL_ACTION);
            var withdrawalAction = context.CurrencyActions.FirstOrDefault(a => a.Name == SeedFinanceData.WITHDRAWAL_ACTION);

            var portfolios = new List<Core.Entities.Finance.Portfolio>()
            {
                new Core.Entities.Finance.Portfolio()
                {
                    Id = 1,
                    Name = "Сбербанк онлайн",
                    UserId = 1,
                },
                new Core.Entities.Finance.Portfolio()
                {
                    Id = 2,
                    Name = "Тинькофф",
                    UserId = 1,
                },
                new Core.Entities.Finance.Portfolio()
                {
                    Id = 3,
                    Name = "Другой пользователь",
                    UserId = 2,
                }
            };

            var operations = new List<AssetOperation>()
            {
                //price = 407800
                //profit = 95370
                new AssetOperation()
                {
                    Id = 1,
                    Ticket = "YNDX",
                    Amount = 2,
                    PaymentPrice = 624860,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id,
                    Date = new DateTime(2019, 11, 18),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                new AssetOperation()
                {
                    Id = 2,
                    Ticket = "YNDX",
                    Amount = 1,
                    PaymentPrice = 312430,
                    AssetAction = sellAction,
                    AssetActionId = sellAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id,
                    Date = new DateTime(2020, 2, 4),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                //price = 21779
                //profit = -11 37744
                new AssetOperation()
                {
                    Id = 3,
                    Ticket = "SBER",
                    Amount = 3,
                    PaymentPrice = 1012430,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id,
                    Date = new DateTime(2018, 1, 4),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                new AssetOperation()
                {
                    Id = 4,
                    Ticket = "SBER",
                    Amount = 1,
                    PaymentPrice = 212430,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id,
                    Date = new DateTime(2019, 4, 4),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                //price = 21779
                //profit = 21779 - 21430
                new AssetOperation()
                {
                    Id = 5,
                    Ticket = "SBER",
                    Amount = 1,
                    PaymentPrice = 21430,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id,
                    Date = new DateTime(2019, 1, 4),
                    PortfolioId = portfolios[1].Id,
                    Portfolio = portfolios[1]
                },
                // all profit = -1042025

                new AssetOperation()
                {
                    Id = 6,
                    Ticket = "SBER",
                    Amount = 1,
                    PaymentPrice = 21430,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = stockType,
                    AssetTypeId = stockType.Id,
                    Date = new DateTime(2019, 1, 4),
                    PortfolioId = portfolios[2].Id,
                    Portfolio = portfolios[2]
                }
            };

            var currencyOperations = new List<CurrencyOperation>()
            {
                new CurrencyOperation()
                {
                    Id = 1,
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0],
                    Date = new DateTime(2018, 1, 8),
                    CurrencyId = "RUR",
                    CurrencyName = "₽",
                    CurrencyAction = refillAction,
                    CurrencyActionId = refillAction.Id,
                    Price = 2000000
                },
                new CurrencyOperation()
                {
                    Id = 2,
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0],
                    Date = new DateTime(2018, 1, 8),
                    CurrencyId = "RUR",
                    CurrencyName = "₽",
                    CurrencyAction = withdrawalAction,
                    CurrencyActionId = withdrawalAction.Id,
                    Price = 200000
                },
                new CurrencyOperation()
                {
                    Id = 3,
                    PortfolioId = portfolios[1].Id,
                    Portfolio = portfolios[1],
                    Date = new DateTime(2018, 1, 10),
                    CurrencyId = "RUR",
                    CurrencyName = "₽",
                    CurrencyAction = refillAction,
                    CurrencyActionId = refillAction.Id,
                    Price = 50000
                },
                new CurrencyOperation()
                {
                    Id = 4,
                    PortfolioId = portfolios[2].Id,
                    Portfolio = portfolios[2],
                    Date = new DateTime(2018, 1, 10),
                    CurrencyId = "RUR",
                    CurrencyName = "₽",
                    CurrencyAction = refillAction,
                    CurrencyActionId = refillAction.Id,
                    Price = 50000
                }
            };

            context.CurrencyOperations.AddRange(currencyOperations);
            context.AssetOperations.AddRange(operations);
            context.SaveChanges();
        }

        public static void MockStockData(MockHttpMessageHandler mockHttp)
        {
            var jsonYNDX = File.ReadAllTextAsync("TestData/stock_response_YNDX.json").Result;
            var jsonSBER = File.ReadAllTextAsync("TestData/stock_response_SBER.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX.json")
                .Respond("application/json", jsonYNDX);

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/SBER.json")
                .Respond("application/json", jsonSBER);
        }

        public static void MockDividendData(MockHttpMessageHandler mockHttp)
        {
            var jsonDivsSBER = File.ReadAllTextAsync("TestData/dividends_response_SBER.json").Result;
            var jsonDivsYNDX = File.ReadAllTextAsync("TestData/dividends_response_YNDX.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/securities/SBER/dividends.json")
                .Respond("application/json", jsonDivsSBER);

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/securities/YNDX/dividends.json")
                .Respond("application/json", jsonDivsYNDX);
        }
    }
}