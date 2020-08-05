using System.Collections.Generic;
using System.Text.Json;
using Portfolio.Finance.Services.Entities;

namespace Portfolio.Finance.Services
{
    public static class FinanceHelpers
    {
        public static double GetPriceDouble(int price)
        {
            var whole = price / 100;
            var fraction = 0.01 * (price % 100);
            return whole + fraction;
        }

        public static JsonElement GetValueOfColumnMarketdata(string column, AssetResponse data)
        {
            var index = data.marketdata.columns.IndexOf(column);

            if (index != -1)
            {
                return data.marketdata.data[0][index];
            }

            return new JsonElement();
        }

        public static JsonElement GetValueOfColumnSecurities(string column, AssetResponse data)
        {
            var index = data.securities.columns.IndexOf(column);

            if (index != -1)
            {
                return data.securities.data[0][index];
            }

            return new JsonElement();
        }
    }
}