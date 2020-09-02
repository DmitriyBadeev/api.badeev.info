using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Services;

namespace Portfolio.Finance.Services.Interfaces
{
    public interface IGraphService
    {
        Task<List<StockCandle>> StockCandles(string ticket, DateTime from, CandleInterval interval);
    }
}