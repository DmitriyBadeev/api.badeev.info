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

namespace Portfolio.Finance.Services.Test.EntitiesTests
{
    [TestFixture]
    public class BondInfoTests
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

        private AssetType _bondType = new AssetType()
        {
            Id = 1,
            Name = SeedFinanceData.BOND_ASSET_TYPE
        };

        [SetUp]
        public void Setup()
        {
            var mockHttp = new MockHttpMessageHandler();
            var client = mockHttp.ToHttpClient();
            TestHelpers.MockBondData(mockHttp);
            TestHelpers.MockCouponsData(mockHttp);
            var stockMarketAPI = new StockMarketAPI(client);
            _stockMarketData = new StockMarketData(stockMarketAPI);
        }

        [Test]
        public async Task GetAllPrice()
        {
            var bond = Getbond();

            var price = await bond.GetAllPrice();

            Assert.AreEqual(318849, price);
        }

        [Test]
        public void AmortizationDate()
        {
            var bond = (BondInfo)Getbond();
            var amortizedBond = (BondInfo)GetAmortizedBond();

            Assert.AreEqual(new DateTime(2022, 7, 20), bond.AmortizationDate);
            Assert.AreEqual(new DateTime(2019, 12, 11), amortizedBond.AmortizationDate);
        }

        [Test]
        public void HasAmortized()
        {
            var bond = (BondInfo)Getbond();
            var amortizedBond = (BondInfo)GetAmortizedBond();

            Assert.IsFalse(bond.HasAmortized);
            Assert.IsTrue(amortizedBond.HasAmortized);
        }

        [Test]
        public void PaymentsData()
        {
            var bond = (BondInfo)Getbond();
            var amortizedBond = (BondInfo)GetAmortizedBond();

            Assert.AreEqual(6, bond.PaymentsData.Count);
            Assert.AreEqual(5, amortizedBond.PaymentsData.Count);
        }

        [Test]
        public void GetSumPayments()
        {
            var bond = (BondInfo)Getbond();
            var amortizedBond = (BondInfo)GetAmortizedBond();

            Assert.AreEqual(3790 * 3, bond.GetSumPayments());
            Assert.AreEqual(113564 * 2, amortizedBond.GetSumPayments());
        }

        private AssetInfo Getbond()
        {
            var operations = new List<AssetOperation>()
            {
                new AssetOperation()
                {
                    Id = 1,
                    Ticket = "SU26209RMFS5",
                    Amount = 2,
                    PaymentPrice = 103180,
                    AssetAction = _buyAction,
                    AssetActionId = _buyAction.Id,
                    AssetType = _bondType,
                    AssetTypeId = _bondType.Id,
                    Date = new DateTime(2020, 2, 7)
                },
                new AssetOperation()
                {
                    Id = 2,
                    Ticket = "SU26209RMFS5",
                    Amount = 1,
                    PaymentPrice = 103230,
                    AssetAction = _buyAction,
                    AssetActionId = _buyAction.Id,
                    AssetType = _bondType,
                    AssetTypeId = _bondType.Id,
                    Date = new DateTime(2020, 4, 4)
                }
            };

            var stockInfo = new BondInfo(_stockMarketData, "SU26209RMFS5");
            foreach (var assetOperation in operations)
            {
                stockInfo.RegisterOperation(assetOperation);
            }

            return stockInfo;
        }

        private AssetInfo GetAmortizedBond()
        {
            var operations = new List<AssetOperation>()
            {
                new AssetOperation()
                {
                    Id = 1,
                    Ticket = "SU26210RMFS3",
                    Amount = 2,
                    PaymentPrice = 101180,
                    AssetAction = _buyAction,
                    AssetActionId = _buyAction.Id,
                    AssetType = _bondType,
                    AssetTypeId = _bondType.Id,
                    Date = new DateTime(2018, 2, 7)
                },
            };

            var stockInfo = new BondInfo(_stockMarketData, "SU26210RMFS3");
            foreach (var assetOperation in operations)
            {
                stockInfo.RegisterOperation(assetOperation);
            }

            return stockInfo;
        }
    }
}