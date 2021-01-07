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

        public static void SeedApp(FinanceDbContext context)
        {
            var mockLogger = new Mock<ILogger<SeedFinanceDataService>>();
            var seedService = new SeedFinanceDataService(mockLogger.Object, context);

            seedService.Initialise();
        }
        
        public static void SeedOperations1(FinanceDbContext context)
        {
            SeedApp(context);
            var buyAction = context.AssetActions.FirstOrDefault(a => a.Name == SeedFinanceData.BUY_ACTION);
            var sellAction = context.AssetActions.FirstOrDefault(a => a.Name == SeedFinanceData.SELL_ACTION);

            var stockType = context.AssetTypes.FirstOrDefault(a => a.Name == SeedFinanceData.STOCK_ASSET_TYPE);
            var fondType = context.AssetTypes.FirstOrDefault(a => a.Name == SeedFinanceData.FOND_ASSET_TYPE);
            var bondType = context.AssetTypes.FirstOrDefault(a => a.Name == SeedFinanceData.BOND_ASSET_TYPE);

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
                //price = 434720
                //profit = 95370
                new AssetOperation()
                {
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
                //price = 22658
                //profit = -11 37744
                new AssetOperation()
                {
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
                //price = 101840
                //profit = 101840 - 81840 = 20000
                new AssetOperation()
                {
                    Ticket = "FXGD",
                    Amount = 1,
                    PaymentPrice = 81840,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = fondType,
                    AssetTypeId = fondType.Id,
                    Date = new DateTime(2019, 4, 4),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                new AssetOperation()
                {
                    Ticket = "SU26209RMFS5",
                    Amount = 1,
                    PaymentPrice = 103521,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = bondType,
                    AssetTypeId = bondType.Id,
                    Date = new DateTime(2020, 2, 7),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                new AssetOperation()
                {
                    Ticket = "SU26210RMFS3",
                    Amount = 1,
                    PaymentPrice = 102100,
                    AssetAction = buyAction,
                    AssetActionId = buyAction.Id,
                    AssetType = bondType,
                    AssetTypeId = bondType.Id,
                    Date = new DateTime(2018, 2, 7),
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0]
                },
                //price = 22658
                //profit = 22658 - 21430
                new AssetOperation()
                {
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
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0],
                    Date = new DateTime(2018, 1, 8),
                    CurrencyId = SeedFinanceData.RUB_CURRENCY_ID,
                    CurrencyName = SeedFinanceData.RUB_CURRENCY_NAME,
                    CurrencyAction = refillAction,
                    CurrencyActionId = refillAction.Id,
                    Price = 2500000
                },
                new CurrencyOperation()
                {
                    PortfolioId = portfolios[0].Id,
                    Portfolio = portfolios[0],
                    Date = new DateTime(2018, 1, 8),
                    CurrencyId = SeedFinanceData.RUB_CURRENCY_ID,
                    CurrencyName = SeedFinanceData.RUB_CURRENCY_NAME,
                    CurrencyAction = withdrawalAction,
                    CurrencyActionId = withdrawalAction.Id,
                    Price = 200000
                },
                new CurrencyOperation()
                {
                    PortfolioId = portfolios[1].Id,
                    Portfolio = portfolios[1],
                    Date = new DateTime(2018, 1, 10),
                    CurrencyId = SeedFinanceData.RUB_CURRENCY_ID,
                    CurrencyName = SeedFinanceData.RUB_CURRENCY_NAME,
                    CurrencyAction = refillAction,
                    CurrencyActionId = refillAction.Id,
                    Price = 50000
                },
                new CurrencyOperation()
                {
                    PortfolioId = portfolios[2].Id,
                    Portfolio = portfolios[2],
                    Date = new DateTime(2018, 1, 10),
                    CurrencyId = SeedFinanceData.RUB_CURRENCY_ID,
                    CurrencyName = SeedFinanceData.RUB_CURRENCY_NAME,
                    CurrencyAction = refillAction,
                    CurrencyActionId = refillAction.Id,
                    Price = 50000
                }
            };
            
            var payments = new List<Payment>()
            {
                new Payment()
                {
                    PortfolioId = 1,
                    Ticket = "SBER",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 10000
                },
                new Payment()
                {
                    PortfolioId = 1,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 5000
                },
                new Payment()
                {
                    PortfolioId = 1,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 5000
                },
                new Payment()
                {
                    PortfolioId = 2,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 5000
                },
                new Payment()
                {
                    PortfolioId = 3,
                    Ticket = "SBERP",
                    Amount = 10,
                    Date = DateTime.Now,
                    PaymentValue = 10000
                }
            };

            context.Portfolios.AddRange(portfolios);
            context.CurrencyOperations.AddRange(currencyOperations);
            context.AssetOperations.AddRange(operations);
            context.Payments.AddRange(payments);
            context.SaveChanges();
        }

        public static void MockStockData(MockHttpMessageHandler mockHttp)
        {
            var jsonYNDX = File.ReadAllTextAsync("TestData/AssetsData/stock_response_YNDX.json").Result;
            var jsonSBER = File.ReadAllTextAsync("TestData/AssetsData/stock_response_SBER.json").Result;
            
            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/YNDX.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonYNDX);

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQBR/securities/SBER.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonSBER);
        }

        public static void MockFondData(MockHttpMessageHandler mockHttp)
        {
            var jsonFXGD = File.ReadAllTextAsync("TestData/AssetsData/fond_response_FXGD.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/boards/TQTF/securities/FXGD.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonFXGD);
        }

        public static void MockDividendData(MockHttpMessageHandler mockHttp)
        {
            var jsonDivsSBER = File.ReadAllTextAsync("TestData/DividendsData/dividends_response_SBER.json").Result;
            var jsonDivsYNDX = File.ReadAllTextAsync("TestData/DividendsData/dividends_response_YNDX.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/securities/SBER/dividends.json?iss.meta=off")
                .Respond("application/json", jsonDivsSBER);

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/securities/YNDX/dividends.json?iss.meta=off")
                .Respond("application/json", jsonDivsYNDX);
        }

        public static void MockBondData(MockHttpMessageHandler mockHttp)
        {
            var jsonBond = File.ReadAllTextAsync("TestData/AssetsData/bond_response_SU26209RMFS5.json").Result;
            var jsonAmortizedBond = File.ReadAllTextAsync("TestData/AssetsData/bond_response_SU26210RMFS3_2019.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/bonds/boards/TQOB/securities/SU26209RMFS5.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonBond);

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/bonds/boards/TQOB/securities/SU26210RMFS3.json?iss.meta=off&iss.only=securities,marketdata")
                .Respond("application/json", jsonAmortizedBond);
        }

        public static void MockCouponsData(MockHttpMessageHandler mockHttp)
        {
            var jsonCoupon = File.ReadAllTextAsync("TestData/CouponsData/coupons_response_RU000A0JSMA2.json").Result;
            var jsonAmortizedCoupon = File.ReadAllTextAsync("TestData/CouponsData/coupons_response_RU000A0JTG59_2019.json").Result;

            mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/statistics/engines/stock/markets/bonds/bondization/SU26209RMFS5.json?from=2020-02-07&iss.only=coupons,amortizations&iss.meta=off")
                .Respond("application/json", jsonCoupon);

            mockHttp
                .When(HttpMethod.Get, "https://iss.moex.com/iss/statistics/engines/stock/markets/bonds/bondization/SU26210RMFS3.json?from=2018-02-07&iss.only=coupons,amortizations&iss.meta=off")
                .Respond("application/json", jsonAmortizedCoupon);
        }

        public static void MockCandles(MockHttpMessageHandler mockHttp)
        {
            var json = File.ReadAllTextAsync("TestData/GraphsData/stock_candle_response_2020-06-02_24.json").Result;

            mockHttp
                .When(HttpMethod.Get, "http://iss.moex.com/iss/engines/stock/markets/shares/securities/YNDX/candles.json?from=2020-06-02&interval=24&iss.meta=off")
                .Respond("application/json", json);
        }
    }
}