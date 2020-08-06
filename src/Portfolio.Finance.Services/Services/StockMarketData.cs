using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
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

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<AssetResponse>(response.JsonContent);
                return data;
            }

            return null;
        }

        public async Task<AssetResponse> GetFondData(string codeFond)
        {
            var response = await _stockMarketApi.FindFond(codeFond);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<AssetResponse>(response.JsonContent);
                return data;
            }

            return null;
        }

        public async Task<AssetResponse> GetBondData(string codeBond)
        {
            var response = await _stockMarketApi.FindBond(codeBond);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<AssetResponse>(response.JsonContent);
                return data;
            }

            return null;
        }

        public async Task<DividendsResponse> GetDividendsData(string codeStock)
        {
            var response = await _stockMarketApi.FindDividends(codeStock);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<DividendsResponse>(response.JsonContent);
                return data;
            }

            return null;
        }

        public async Task<CouponsResponse> GetCouponsData(string codeBond, DateTime boughtDate)
        {
            var response = await _stockMarketApi.FindCoupons(codeBond, boughtDate);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<CouponsResponse>(response.JsonContent);
                return data;
            }

            return null;
        }
    }
}
