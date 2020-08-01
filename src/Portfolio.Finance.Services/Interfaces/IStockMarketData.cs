using System.Threading.Tasks;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IStockMarketData
    {
        Task<StockResponse> GetStockData(string codeStock);
        Task<DividendsResponse> GetDividendsData(string codeStock);
    }
}