using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Core.Interfaces;

namespace Portfolio.Infrastructure.Services
{
    public static class ServicesCollectionExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<ApplicationDataService>();
            services.AddScoped<ISeedDataService, SeedDataService>();

            return services;
        }
    }
}
