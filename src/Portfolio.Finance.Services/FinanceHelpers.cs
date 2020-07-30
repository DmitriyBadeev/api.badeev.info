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

        public static string GetValueOfColumn(string column, StockResponse data)
        {
            var index = data.marketdata.columns.IndexOf(column);


            if (index != -1)
            {
                return data.marketdata.data[1][index].ToString();
            }

            return null;
        }
    }
}