using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO.Responses;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class StockMarketData : IStockMarketData
    {
        private readonly IStockMarketAPI _stockMarketApi;

        public StockMarketData(IStockMarketAPI stockMarketApi)
        {
            _stockMarketApi = stockMarketApi;
        }

        public async Task<AssetResponse> GetStockData(string codeStock)
        {
            var response = await _stockMarketApi.FindStock(codeStock);

            return GetData<AssetResponse>(response);
        }

        public async Task<AssetResponse> GetFondData(string codeFond)
        {
            var response = await _stockMarketApi.FindFond(codeFond);

            return GetData<AssetResponse>(response);
        }

        public async Task<AssetResponse> GetBondData(string codeBond)
        {
            var response = await _stockMarketApi.FindBond(codeBond);

            return GetData<AssetResponse>(response);
        }

        public async Task<AssetResponse> GetIndexData(string codeIndex)
        {
            var response = await _stockMarketApi.FindIndex(codeIndex);

            return GetData<AssetResponse>(response);
        }

        public async Task<AssetResponse> GetCurrencyData(string codeCurrency)
        {
            var response = await _stockMarketApi.FindCurrency(codeCurrency);

            return GetData<AssetResponse>(response);
        }

        public async Task<DividendsResponse> GetDividendsData(string codeStock)
        {
            var response = await _stockMarketApi.FindDividends(codeStock);
            
            return GetData<DividendsResponse>(response);
        }

        public async Task<CouponsResponse> GetCouponsData(string codeBond, DateTime boughtDate)
        {
            var response = await _stockMarketApi.FindCoupons(codeBond, boughtDate);

            return GetData<CouponsResponse>(response);
        }

        public async Task<AssetResponse> GetBrentData()
        {
            var response = await _stockMarketApi.FindBrent();

            return GetData<AssetResponse>(response);
        }

        public async Task<SearchResponse> GetSearchData(string code)
        {
            var response = await _stockMarketApi.Search(code);

            return GetData<SearchResponse>(response);
        }

        private TResponse GetData<TResponse>(ApiResponse response) where TResponse : class
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<TResponse>(response.JsonContent);
                return data;
            }

            return null;
        }
    }
}
