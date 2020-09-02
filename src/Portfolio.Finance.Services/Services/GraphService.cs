using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;

namespace Portfolio.Finance.Services.Services
{
    public class GraphService : IGraphService
    {
        private readonly IStockMarketData _marketData;

        public GraphService(IStockMarketData marketData)
        {
            _marketData = marketData;
        }

        public async Task<List<StockCandle>> StockCandles(string ticket, DateTime from, CandleInterval interval)
        {
            var data = await _marketData.GetStockCandleData(ticket, from, interval);

            var openIndex = data.candles.columns.IndexOf("open");
            var closeIndex = data.candles.columns.IndexOf("close");
            var highIndex = data.candles.columns.IndexOf("high");
            var lowIndex = data.candles.columns.IndexOf("low");
            var valueIndex = data.candles.columns.IndexOf("value");
            var volumeIndex = data.candles.columns.IndexOf("volume");
            var dateTimeIndex = data.candles.columns.IndexOf("begin");

            var candles = new List<StockCandle>();

            foreach (var candleData in data.candles.data)
            {
                candles.Add(new StockCandle()
                {
                    Open = candleData[openIndex].GetDouble(),
                    Close = candleData[closeIndex].GetDouble(),
                    High = candleData[highIndex].GetDouble(),
                    Low = candleData[lowIndex].GetDouble(),
                    Value = candleData[valueIndex].GetDouble(),
                    Volume = candleData[volumeIndex].GetDouble(),
                    DateTime = DateTime.Parse(candleData[dateTimeIndex].GetString()),
                });
            }

            return candles;
        }
    }
}
