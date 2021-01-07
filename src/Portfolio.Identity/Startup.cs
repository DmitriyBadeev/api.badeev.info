using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Portfolio.Identity
{
    public class Startup
    {
        private readonly IHostingEnvironment _environment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            _environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var login = Configuration["login"];
            var password = Configuration["password"];
            var commonClientId = Configuration["client_id"];
            var financeClientId = Configuration["financeClientId"];
            var redirects = Configuration.GetSection("redirect_uris").Get<List<string>>();
            var financeRedirects = Configuration.GetSection("finance_redirect_uris").Get<List<string>>();
            var apiPortfolio = Configuration["apiPortfolio"];
            var apiFinance = Configuration["apiFinance"];
        
            var rsa = new RsaKeyService(_environment, TimeSpan.FromDays(120));
            services.AddSingleton(provider => rsa);

            services.AddMvc();
            services.AddIdentityServer()
                .AddSigningCredential(rsa.GetKey())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources(apiPortfolio, apiFinance))
                .AddInMemoryClients(Config.GetSpaClient(commonClientId, financeClientId, 
                    redirects, financeRedirects, apiPortfolio, apiFinance))
                .AddTestUsers(Config.GetUser(login, password));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
