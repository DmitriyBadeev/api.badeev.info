﻿using System;
using System.Text.Json;
using Portfolio.Finance.Services.DTO;

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

        public static int GetPriceInt(double price)
        {
            return (int) Math.Round(price * 100);
        }

        public static JsonElement GetValueOfColumnMarketdata(string column, AssetResponse data)
        {
            var index = data.marketdata.columns.IndexOf(column);

            if (index != -1 && data.marketdata.data.Count > 0)
            {
                return data.marketdata.data[0][index];
            }

            return new JsonElement();
        }

        public static JsonElement GetValueOfColumnSecurities(string column, AssetResponse data)
        {
            var index = data.securities.columns.IndexOf(column);

            if (index != -1 && data.securities.data.Count > 0)
            {
                return data.securities.data[0][index];
            }

            return new JsonElement();
        }
    }
}