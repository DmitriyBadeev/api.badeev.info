using System;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Finance.Services.Interfaces;
using Portfolio.Finance.Services.Services;

namespace Portfolio.Finance.Services
{
    public static class ServicesCollectionExtension
    {
        public static IServiceCollection AddFinanceServices(this IServiceCollection services)
        {
            services.AddScoped<IStockMarketAPI, StockMarketAPI>();
            services.AddScoped<IStockMarketData, StockMarketData>();
            return services;
        }
    }
}
