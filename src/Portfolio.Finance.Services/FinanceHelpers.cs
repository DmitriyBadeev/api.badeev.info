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

        public static JsonElement GetValueOfColumn(string column, List<JsonElement> stockInfo, StockResponse data)
        {
            var index = data.marketdata.columns.IndexOf(column);

            if (index != -1)
            {
                return stockInfo[index];
            }

            return new JsonElement();
        }

        public static List<JsonElement> GetStockInfo(string boardId, StockResponse data)
        {
            var indexOfBoardId = data.marketdata.columns.IndexOf("BOARDID");
            return data.marketdata.data.Find(el => el[indexOfBoardId].GetString() == boardId);
        }
    }
}