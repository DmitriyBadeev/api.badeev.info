using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;
using Portfolio.Infrastructure.Services;
using RichardSzalay.MockHttp;

namespace Portfolio.Finance.Services.Test
{
    [TestFixture]
    public class StockInfoTests
    {
        private IStockMarketData _stockMarketData;
        private AssetAction _buyAction = new AssetAction()
        {
            Id = 1,
            Name = SeedFinanceData.BUY_ACTION
        };

        private AssetAction _sellAction = new AssetAction()
        {
            Id = 1,
            Name = SeedFinanceData.SELL_ACTION
        };

        private AssetType _stockType = new AssetType()
        {
            Id = 1,
            Name = SeedFinanceData.STOCK_ASSET_TYPE
        };

        [SetUp]
        public void Setup()
        { 
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();
            TestHelpers.MockStockData(mockHttp);
            TestHelpers.MockDividendData(mockHttp);
            var stockMarketAPI = new StockMarketAPI(client);
            _stockMarketData = new StockMarketData(stockMarketAPI);
        }

        [Test]
        public async Task GetPrice1()
        {
            var stockInfo = GetYNDXStock();
            var price = await stockInfo.GetPrice();

            Assert.AreEqual(434720, price);
        }

        [Test]
        public async Task GetPrice2()
        {
            var stockInfo = GetSBERStock();
            var price = await stockInfo.GetPrice();

            Assert.AreEqual(22658, price);
        }

        [Test]
        public async Task GetPaperProfit1()
        {
            var stockInfo = GetYNDXStock();
            var profit = await stockInfo.GetPaperProfit();

            Assert.AreEqual(434720 - 312430, profit);
        }

        [Test]
        public async Task GetPaperProfit2()
        {
            var stockInfo = GetSBERStock();
            var profit = await stockInfo.GetPaperProfit();

            Assert.AreEqual(-1134228, profit);
        }

        [Test]
        public async Task NameAsset()
        {
            var stockInfo = GetSBERStock();
            var name = await stockInfo.GetName();

            Assert.AreEqual("Сбербанк".Length, name.Length);
        }

        [Test]
        public void GetDividendsData()
        {
            var stockSBERInfo = GetSBERStock();
            var stockYNDXInfo = GetYNDXStock();

            Assert.AreEqual(7, stockSBERInfo.PaymentsData.Count);
            Assert.AreEqual(0, stockYNDXInfo.PaymentsData.Count);

            Assert.AreEqual("RUB", stockSBERInfo.PaymentsData[0].CurrencyId);
            Assert.AreEqual(320, stockSBERInfo.PaymentsData[0].PaymentValue);
            Assert.AreEqual(new DateTime(2014, 6, 17), stockSBERInfo.PaymentsData[0].RegistryCloseDate);
            Assert.AreEqual("SBER", stockSBERInfo.PaymentsData[0].Ticket);
        }

        [Test]
        public void PaymentsData()
        {
            var stockSBERInfo = GetSBERStock();
            var stockYNDXInfo = GetYNDXStock();

            Assert.AreEqual(1, stockSBERInfo.GetFuturePayment().Count);
            Assert.AreEqual(1870, stockSBERInfo.GetFuturePayment()[0].PaymentValue);
            Assert.AreEqual(0, stockYNDXInfo.GetFuturePayment().Count);
        }

        [Test]
        public void GetPaidPayments()
        {
            var stockSBERInfo = GetSBERStock();
            var stockYNDXInfo = GetYNDXStock();

            Assert.AreEqual(2, stockSBERInfo.GetPaidPayments().Count);
            Assert.AreEqual(0, stockYNDXInfo.GetPaidPayments().Count);
        }

        [Test]
        public void GetSumPayments()
        {
            var stockSBERInfo = GetSBERStock();
            var stockYNDXInfo = GetYNDXStock();

            Assert.AreEqual(3600 + 6400, stockSBERInfo.GetSumPayments());
            Assert.AreEqual(0, stockYNDXInfo.GetSumPayments());
        }

        private AssetInfo GetYNDXStock()
        {
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
                    AssetAction = _buyAction,
                    AssetActionId = _buyAction.Id,
                    AssetType = _stockType,
                    AssetTypeId = _stockType.Id,
                    Date = new DateTime(2019, 11, 18)
                },
                new AssetOperation()
                {
                    Id = 2,
                    Ticket = "YNDX",
                    Amount = 1,
                    PaymentPrice = 312430,
                    AssetAction = _sellAction,
                    AssetActionId = _sellAction.Id,
                    AssetType = _stockType,
                    AssetTypeId = _stockType.Id,
                    Date = new DateTime(2020, 2, 4)
                },
            };

            var stockInfo = new StockInfo(_stockMarketData, "YNDX");

            foreach (var assetOperation in operations)
            {
                stockInfo.RegisterOperation(assetOperation);
            }
            
            return stockInfo;
        }

        private AssetInfo GetSBERStock()
        {
            var operations = new List<AssetOperation>()
            {
                new AssetOperation()
                {
                    Id = 3,
                    Ticket = "SBER",
                    Amount = 3,
                    PaymentPrice = 1012430,
                    AssetAction = _buyAction,
                    AssetActionId = _buyAction.Id,
                    AssetType = _stockType,
                    AssetTypeId = _stockType.Id,
                    Date = new DateTime(2018, 1, 4)
                },
                new AssetOperation()
                {
                    Id = 4,
                    Ticket = "SBER",
                    Amount = 1,
                    PaymentPrice = 212430,
                    AssetAction = _buyAction,
                    AssetActionId = _buyAction.Id,
                    AssetType = _stockType,
                    AssetTypeId = _stockType.Id,
                    Date = new DateTime(2019, 4, 4)
                }
            };
            
            var stockInfo = new StockInfo(_stockMarketData, "SBER");
            foreach (var assetOperation in operations)
            {
                stockInfo.RegisterOperation(assetOperation);
            }

            return stockInfo;
        }
    }
}