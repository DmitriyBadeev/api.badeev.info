using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IMarketQuotesService
    {
        Task<CommonMarketQuote> GetIMOEX();
        Task<CommonMarketQuote> GetRTSI();
        Task<CommonMarketQuote> GetUSD();
        Task<CommonMarketQuote> GetEURO();
        Task<CommonMarketQuote> GetBrent();
        IEnumerable<CommonMarketQuote> GetMarketQuotes();
    }
}