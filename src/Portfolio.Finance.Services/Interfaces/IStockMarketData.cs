using System;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO.Responses;
using Portfolio.Finance.Services.Services;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IStockMarketData
    {
        Task<AssetResponse> GetStockData(string codeStock);
        Task<DividendsResponse> GetDividendsData(string codeStock);
        Task<AssetResponse> GetFondData(string codeFond);
        Task<AssetResponse> GetBondData(string codeBond);
        Task<AssetResponse> GetIndexData(string codeIndex);
        Task<AssetResponse> GetCurrencyData(string codeCurrency);
        Task<CouponsResponse> GetCouponsData(string codeBond, DateTime boughtDate);
        Task<AssetResponse> GetBrentData();
        Task<StockCandleResponse> GetStockCandleData(string code, DateTime from, CandleInterval interval);
        Task<SearchResponse> GetSearchData(string code);
    }
}