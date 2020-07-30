using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class StockMarketData : IStockMarketData
    {
        private readonly StockMarketAPI _stockMarketApi;

        public StockMarketData(StockMarketAPI stockMarketApi)
        {
            _stockMarketApi = stockMarketApi;
        }

        public async Task<StockResponse> GetStockData(string codeStock)
        {
            var response = await _stockMarketApi.FindStock(codeStock);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var data = JsonSerializer.Deserialize<StockResponse>(response.JsonContent);
                return data;
            }

            return null;
        }
    }
}
