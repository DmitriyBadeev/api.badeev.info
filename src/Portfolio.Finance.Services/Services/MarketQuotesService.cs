using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class MarketQuotesService : IMarketQuotesService
    {
        private readonly IStockMarketData _marketData;

        public MarketQuotesService(IStockMarketData marketData)
        {
            _marketData = marketData;
        }

        public IEnumerable<CommonMarketQuote> GetMarketQuotes()
        {
            yield return GetUSD().Result;
            yield return GetEURO().Result;
            yield return GetIMOEX().Result;
            yield return GetRTSI().Result;
            yield return GetBrent().Result;
        }

        public async Task<CommonMarketQuote> GetIMOEX()
        {
            return await GetIndex("IMOEX");
        }

        public async Task<CommonMarketQuote> GetRTSI()
        {
            return await GetIndex("RTSI");
        }

        public async Task<CommonMarketQuote> GetUSD()
        {
            return await GetCurrency("USD000UTSTOM", "USD");
        }

        public async Task<CommonMarketQuote> GetEURO()
        {
            return await GetCurrency("EUR_RUB__TOM", "EURO");
        }

        public async Task<CommonMarketQuote> GetBrent()
        {
            var brentData = await _marketData.GetBrentData();

            var name = "Нефть";
            var value = FinanceHelpers.GetValueOfColumnMarketdata("LAST", brentData).GetDouble();
            var change = FinanceHelpers.GetValueOfColumnMarketdata("SETTLETOPREVSETTLEPRC", brentData).GetDouble();
            var time = FinanceHelpers.GetValueOfColumnMarketdata("TIME", brentData).GetString();

            return new CommonMarketQuote()
            {
                Ticket = "BRU0",
                Name = name,
                Change = FinanceHelpers.NormalizeDouble(change),
                Value = FinanceHelpers.NormalizeDouble(value),
                Time = time
            };
        }

        private async Task<CommonMarketQuote> GetIndex(string ticket)
        {
            var indexData = await _marketData.GetIndexData(ticket);

            var name = FinanceHelpers.GetValueOfColumnSecurities("SHORTNAME", indexData).GetString();
            var value = FinanceHelpers.GetValueOfColumnMarketdata("LASTVALUE", indexData).GetDouble();
            var change = FinanceHelpers.GetValueOfColumnMarketdata("LASTCHANGETOOPENPRC", indexData).GetDouble();
            var time = FinanceHelpers.GetValueOfColumnMarketdata("TIME", indexData).GetString();

            return new CommonMarketQuote()
            {
                Ticket = ticket,
                Name = name,
                Change = FinanceHelpers.NormalizeDouble(change),
                Value = FinanceHelpers.NormalizeDouble(value),
                Time = time
            };
        }

        private async Task<CommonMarketQuote> GetCurrency(string ticket, string name)
        {
            var currencyData = await _marketData.GetCurrencyData(ticket);

            var value = FinanceHelpers.GetValueOfColumnMarketdata("LAST", currencyData).GetDouble();
            var change = FinanceHelpers.GetValueOfColumnMarketdata("CHANGE", currencyData).GetDouble();
            var time = FinanceHelpers.GetValueOfColumnMarketdata("TIME", currencyData).GetString();

            return new CommonMarketQuote()
            {
                Ticket = ticket,
                Name = name,
                Change = FinanceHelpers.NormalizeDouble(change),
                Value = FinanceHelpers.NormalizeDouble(value),
                Time = time
            };
        }
    }
}
