using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class MarketService : IMarketService
    {
        private readonly FinanceDataService _financeDataService;
        private readonly IAssetsFactory _assetsFactory;
        private readonly IBalanceService _balanceService;
        private List<PortfolioData> _portfolios;

        public MarketService(FinanceDataService financeDataService, IAssetsFactory assetsFactory, 
            IBalanceService balanceService)
        {
            _financeDataService = financeDataService;
            _assetsFactory = assetsFactory;
            _balanceService = balanceService;
        }

        public IEnumerable<AssetOperation> GetAllAssetOperations(int portfolioId)
        {
            return _financeDataService.EfContext.AssetOperations.Where(o => o.PortfolioId == portfolioId);
        }

        public async Task<OperationResult> BuyAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date)
        {
            var portfolio = await _financeDataService.EfContext.Portfolios.FindAsync(portfolioId);
            var buyAction =
                await _financeDataService.EfContext.AssetActions.FirstOrDefaultAsync(a =>
                    a.Name == SeedFinanceData.BUY_ACTION);
            var assetType = await _financeDataService.EfContext.AssetTypes.FindAsync(assetTypeId);

            if (!CommonValidate(price, amount, assetType, portfolio, out var message))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = message
                };
            }

            var currentBalance = _balanceService.GetBalance(portfolioId);
            if (price > currentBalance)
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Недостаточно средств"
                };
            }

            var assetOperation = new AssetOperation
            {
                Portfolio = portfolio,
                PortfolioId = portfolioId,
                AssetAction = buyAction,
                AssetActionId = buyAction.Id,
                AssetType = assetType,
                AssetTypeId = assetType.Id,
                Date = date,
                PaymentPrice = price,
                Ticket = ticket,
                Amount = amount
            };

            await _financeDataService.EfContext.AssetOperations.AddAsync(assetOperation);
            await _financeDataService.EfContext.SaveChangesAsync();

            GetPortfoliosData(portfolio.UserId, true);

            return new OperationResult
            {
                IsSuccess = true,
                Message = "Актив куплен успешно"
            };
        }

        public async Task<OperationResult> SellAsset(int portfolioId, string ticket, int price, int amount,
            int assetTypeId, DateTime date)
        {
            var portfolio = await _financeDataService.EfContext.Portfolios.FindAsync(portfolioId);
            var sellAction =
                await _financeDataService.EfContext.AssetActions.FirstOrDefaultAsync(a =>
                    a.Name == SeedFinanceData.SELL_ACTION);
            var assetType = await _financeDataService.EfContext.AssetTypes.FindAsync(assetTypeId);

            if (!CommonValidate(price, amount, assetType, portfolio, out var message))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = message
                };
            }

            if (!HasAsset(portfolioId, amount, ticket, portfolio.UserId))
            {
                return new OperationResult
                {
                    IsSuccess = false,
                    Message = "Такого количества активов нет в наличии"
                };
            }

            var assetOperation = new AssetOperation
            {
                Portfolio = portfolio,
                PortfolioId = portfolioId,
                AssetAction = sellAction,
                AssetActionId = sellAction.Id,
                AssetType = assetType,
                AssetTypeId = assetType.Id,
                Date = date,
                PaymentPrice = price,
                Ticket = ticket,
                Amount = amount
            };

            await _financeDataService.EfContext.AssetOperations.AddAsync(assetOperation);
            await _financeDataService.EfContext.SaveChangesAsync();

            GetPortfoliosData(portfolio.UserId, true);

            return new OperationResult
            {
                IsSuccess = true,
                Message = "Актив продан успешно"
            };
        }

        public int GetAllPaperPrice(int userId)
        {
            return GetPortfoliosData(userId)
                .Aggregate(0, (total, portfolio) => total + portfolio.Assets
                    .Aggregate(0, (totalPortfolio, asset) => totalPortfolio + asset.GetAllPrice().Result));
        }

        public int GetAllPaperProfit(int userId)
        {
            return GetPortfoliosData(userId)
                .Aggregate(0, (total, portfolio) => total + portfolio.Assets
                    .Aggregate(0, (totalPortfolio, asset) => totalPortfolio + asset.GetPaperProfit().Result));
        }

        public int GetAllPaymentProfit(int userId)
        {
            return GetPortfoliosData(userId)
                .Aggregate(0, (total, portfolio) => total + portfolio.Assets
                    .Aggregate(0, (totalPortfolio, asset) => totalPortfolio + asset.GetSumPayments()));
        }

        public int GetAllCost(int userId)
        {
            return GetAllPaperPrice(userId) + GetAllPaymentProfit(userId) +
                   _balanceService.GetAllBalanceUser(userId);
        }

        public double GetPercentOfPaperProfit(int userId)
        {
            return FinanceHelpers.DivWithOneDigitRound(GetAllPaperProfit(userId),
                _balanceService.GetAllInvestSum(userId));
        }

        public double GetPercentOfPaymentProfit(int userId)
        {
            return FinanceHelpers.DivWithOneDigitRound(GetAllPaymentProfit(userId),
                _balanceService.GetAllInvestSum(userId));
        }

        public IEnumerable<StockInfo> GetStocks(int userId, int portfolioId)
        {
            var portfolio = GetPortfoliosData(userId).Find(p => p.Id == portfolioId);

            if (portfolio == null)
            {
                yield break;
            }

            foreach (var asset in portfolio.Assets)
            {
                var type = asset.GetType();

                if (type.Name == "StockInfo")
                    yield return (StockInfo)asset;
            }
        }

        public IEnumerable<FondInfo> GetFonds(int userId, int portfolioId)
        {
            var portfolio = GetPortfoliosData(userId).Find(p => p.Id == portfolioId);

            if (portfolio == null)
            {
                yield break;
            }

            foreach (var asset in portfolio.Assets)
            {
                var type = asset.GetType();

                if (type.Name == "FondInfo")
                    yield return (FondInfo)asset;
            }
        }

        public IEnumerable<BondInfo> GetBonds(int userId, int portfolioId)
        {
            var portfolio = GetPortfoliosData(userId).Find(p => p.Id == portfolioId);

            if (portfolio == null)
            {
                yield break;
            }

            foreach (var asset in portfolio.Assets)
            {
                var type = asset.GetType();

                if (type.Name == "BondInfo")
                    yield return (BondInfo)asset;
            }
        }

        public int GetUserBalanceWithPaidPayments(int userId)
        {
            return _balanceService.GetAllBalanceUser(userId) + GetAllPaymentProfit(userId);
        }

        private bool HasAsset(int portfolioId, int amount, string ticket, int userId)
        {
            var portfolio = GetPortfoliosData(userId).Find(p => p.Id == portfolioId);

            var asset = portfolio.Assets.FirstOrDefault(a => a.Ticket == ticket);

            if (asset == null) return false;
            if (asset.Amount < amount) return false;

            return true;
        }

        private List<PortfolioData> GetPortfoliosData(int userId, bool isForceUpdate = false)
        {
            if (_portfolios == null || isForceUpdate)
            {
                var portfolios = new List<PortfolioData>();
                var userPortfolios = _financeDataService.EfContext.Portfolios
                    .Where(p => p.UserId == userId);

                foreach (var userPortfolio in userPortfolios)
                {
                    var portfolioData = new PortfolioData
                    {
                        Id = userPortfolio.Id,
                        Name = userPortfolio.Name,
                        UserId = userPortfolio.UserId,
                        Assets = _assetsFactory.Create(userPortfolio.Id)
                    };

                    portfolios.Add(portfolioData);
                }

                _portfolios = portfolios;
                return portfolios;
            }

            return _portfolios;
        }

        private static bool CommonValidate(int price, int amount, AssetType assetType,
            Core.Entities.Finance.Portfolio portfolio, out string message)
        {
            if (assetType == null)
            {
                message = "Тип актива не найден";
                return false;
            }

            if (portfolio == null)
            {
                message = "Портфель не найден";
                return false;
            }

            if (price < 0 || amount <= 0)
            {
                message = "Некорректные данные";
                return false;
            }

            message = "";
            return true;
        }
    }
}
