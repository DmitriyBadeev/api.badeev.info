using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Portfolio.Core.Entities.Finance;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.DTO.Responses;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.Services.Services
{
    public class SearchService : ISearchService
    {
        private readonly IStockMarketData _marketData;
        private readonly FinanceDataService _financeData;

        public SearchService(IStockMarketData marketData, FinanceDataService financeData)
        {
            _marketData = marketData;
            _financeData = financeData;
        }

        public async Task<SearchData> Search(string code)
        {
            var response = await _marketData.GetSearchData(code);

            if (response.description.data.Count == 0)
            {
                return null;
            }

            var name = GetValue(response, "SHORTNAME");
            var ticket = GetValue(response, "SECID");
            var type = GetValue(response, "TYPE");
            var typeName = GetValue(response, "TYPENAME");

            return new SearchData()
            {
                Name = name,
                Ticket = ticket,
                Type = type,
                TypeName = typeName
            };
        }

        public async Task<AssetData> GetAssetData(string ticket, int userId)
        {
            var searchData = await Search(ticket);

            if (searchData == null)
            {
                return null;
            }

            if (AssetTypes.STOCK == searchData.Type)
            {
                var stockInfo = new StockInfo(_marketData, _financeData, ticket);
                RegisterAllOperations(stockInfo, userId);
                return await GetAssetData(stockInfo);
            }

            if (AssetTypes.ETF == searchData.Type)
            {
                var fondInfo = new FondInfo(_marketData, _financeData, ticket);
                RegisterAllOperations(fondInfo, userId);
                return await GetAssetData(fondInfo);
            }

            if (AssetTypes.OFZ == searchData.Type)
            {
                var bondInfo = new BondInfo(_marketData, _financeData, ticket);
                RegisterAllOperations(bondInfo, userId);
                return await GetAssetData(bondInfo);
            }

            return null;
        }

        private async Task<AssetData> GetAssetData(AssetInfo assetInfo)
        {
            var name = await assetInfo.GetName();
            var price = await assetInfo.GetPrice();
            var percentChange = await assetInfo.GetPriceChange();
            var allPrice = await assetInfo.GetAllPrice();
            var paperProfit = await assetInfo.GetPaperProfit();
            var paperProfitPercent = await assetInfo.GetPaperProfitPercent();
            var updateTime = await assetInfo.GetUpdateTime();
            var sumPayments = assetInfo.GetSumPayments();

            return new AssetData()
            {
                Name = name,
                Ticket = assetInfo.Ticket,
                Price = price,
                PriceChange = FinanceHelpers.GetPriceDouble(percentChange),
                AllPrice = FinanceHelpers.GetPriceDouble(allPrice),
                BoughtPrice = FinanceHelpers.GetPriceDouble(assetInfo.BoughtPrice),
                Amount = assetInfo.Amount,
                PaidDividends = FinanceHelpers.GetPriceDouble(sumPayments),
                PaperProfit = FinanceHelpers.GetPriceDouble(paperProfit),
                PaperProfitPercent = paperProfitPercent,
                UpdateTime = updateTime,
                NearestDividend = assetInfo.GetNearestPayment(),
                Payments = assetInfo.PaymentsData
            };
        }

        private string GetValue(SearchResponse response, string columnName)
        {
            var nameIndex = response.description.columns.IndexOf("name");
            var valueIndex = response.description.columns.IndexOf("value");

            return response.description.data.FirstOrDefault(el => el[nameIndex] == columnName)?[valueIndex];
        }

        private void RegisterAllOperations(AssetInfo asset, int userId)
        {
            var operations = GetAssetOperations(asset.Ticket, userId);

            foreach (var operation in operations)
            {
                asset.RegisterOperation(operation);
            }
        }

        private IEnumerable<AssetOperation> GetAssetOperations(string ticket, int userId)
        {
            var portfolios = _financeData.EfContext.Portfolios.Where(p => p.UserId == userId);
            return _financeData.EfContext.AssetOperations
                .Where(o => o.Ticket == ticket && portfolios.Any(p => p.Id == o.PortfolioId))
                .Include(o => o.AssetAction)
                .Include(o => o.AssetType); 
        }
    }

    static class AssetTypes
    {
        public const string ETF = "etf_ppif";
        public const string STOCK = "common_share";
        public const string OFZ = "ofz_bond";
    }
}