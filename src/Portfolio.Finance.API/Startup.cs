using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Finance.API.Mutations;
using Portfolio.Finance.API.Queries;
using Portfolio.Finance.Services;
using Portfolio.Infrastructure.Services;

namespace Portfolio.Finance.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFinanceInfrastructureServices(Configuration.GetConnectionString("DefaultConnection"));
            services.AddFinanceServices();
            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddAuthorizeDirectiveType()
                .AddQueryType(d => d.Name("Queries"))
                .AddType<PortfolioQueries>()
                .AddType<ReportQueries>()
                .AddType<BalanceQueries>()
                .AddType<OperationQueries>()
                .AddMutationType(d => d.Name("Mutations"))
                .AddType<AssetMutations>()
                .AddType<PortfolioMutations>()
                .AddType<BalanceMutations>()
                .Create());

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://identity.badeev.info";
                    options.ApiName = "Portfolio.Finance.API";
                });

            services.AddQueryRequestInterceptor(AuthenticationInterceptor());
        }

        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, token) =>
            {
                if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty("currentUserId",
                        int.Parse(context.User.FindFirstValue("sub")));
                }

                return Task.CompletedTask;
            };
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(b => b
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

            app.UseAuthentication();

            app.UsePlayground();
            app.UseGraphQL("/graphql");
        }
    }
}
