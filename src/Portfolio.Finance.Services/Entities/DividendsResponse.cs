using System.Collections.Generic;
using System.Text.Json;

namespace Portfolio.Finance.Services.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Secid
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Isin
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Registryclosedate
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class Value
    {
        public string type { get; set; }
    }

    public class Currencyid
    {
        public string type { get; set; }
        public int bytes { get; set; }
        public int max_size { get; set; }
    }

    public class DividendMetadata
    {
        public Secid secid { get; set; }
        public Isin isin { get; set; }
        public Registryclosedate registryclosedate { get; set; }
        public Value value { get; set; }
        public Currencyid currencyid { get; set; }
    }

    public class Dividends
    {
        public DividendMetadata metadata { get; set; }
        public List<string> columns { get; set; }
        public List<List<JsonElement>> data { get; set; }
    }

    public class DividendsResponse
    {
        public Dividends dividends { get; set; }
    }
}
