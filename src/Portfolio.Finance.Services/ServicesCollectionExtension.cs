using System.Net.Http;
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
            services.AddScoped<IAssetsFactory, AssetsFactory>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<IMarketService, MarketService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<HttpClient>();
            return services;
        }
    }
}
