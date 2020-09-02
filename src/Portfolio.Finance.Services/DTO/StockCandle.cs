using System;

namespace Portfolio.Finance.Services.DTO
{
    public class StockCandle
    {
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Value { get; set; }
        public double Volume { get; set; }
        public DateTime DateTime { get; set; }
    }
}
