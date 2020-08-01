using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Portfolio.Finance.Services.Entities;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class StockMarketAPI : IStockMarketAPI
    {
        private readonly HttpClient _client;

        public StockMarketAPI(HttpClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse> FindStock(string codeStock)
        {
            var url = $"http://iss.moex.com/iss/engines/stock/markets/shares/securities/{codeStock}.json";
            return await RequestTo(url);
        }

        public async Task<ApiResponse> FindDividends(string codeStock)
        {
            var url = $"http://iss.moex.com/iss/securities/{codeStock}/dividends.json";
            return await RequestTo(url);
        }

        private async Task<ApiResponse> RequestTo(string url)
        {
            var response = await _client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return new ApiResponse()
                {
                    JsonContent = responseContent,
                    StatusCode = response.StatusCode
                };
            }

            return new ApiResponse()
            {
                JsonContent = "",
                StatusCode = response.StatusCode
            };
        }
    }
}
