using System.Collections.Generic;
using System.Text.Json;

namespace Portfolio.Finance.Services.DTO.Responses
{
    public class Dividends
    {
        public List<string> columns { get; set; }
        public List<List<JsonElement>> data { get; set; }
    }

    public class DividendsResponse
    {
        public Dividends dividends { get; set; }
    }
}
