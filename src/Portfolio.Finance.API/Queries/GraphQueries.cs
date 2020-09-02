using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Types;
using Portfolio.Finance.Services.DTO;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;

namespace Portfolio.Finance.API.Queries
{
    [ExtendObjectType(Name = "Queries")]
    public class GraphQueries
    {
        [Authorize]
        public async Task<List<StockCandle>> StockCandles([Service] IGraphService graphService,
            string ticket, DateTime from, CandleInterval interval)
        {
            return await graphService.StockCandles(ticket, from, interval);
        }
    }
}