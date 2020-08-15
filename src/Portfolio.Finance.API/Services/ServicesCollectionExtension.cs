using Microsoft.Extensions.DependencyInjection;
using Portfolio.Finance.API.Services.Interfaces;

namespace Portfolio.Finance.API.Services
{
    public static class ServicesCollectionExtension
    {
        public static IServiceCollection AddTimerServices(this IServiceCollection services)
        {
            services.AddSingleton<ITimerService, TimerService>();
            return services;
        }
    }
}
