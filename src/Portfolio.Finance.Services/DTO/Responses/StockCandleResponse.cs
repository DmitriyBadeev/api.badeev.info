using System.Collections.Generic;
using System.Text.Json;

namespace Portfolio.Finance.Services.DTO.Responses
{
    public class Candles
    {
        public List<string> columns { get; set; }
        public List<List<JsonElement>> data { get; set; }
    }

    public class StockCandleResponse
    {
        public Candles candles { get; set; }
    }
}