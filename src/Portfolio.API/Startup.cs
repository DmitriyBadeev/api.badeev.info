using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.API.Mutations;
using Portfolio.API.Queries;
using Portfolio.Infrastructure.Services;

namespace Portfolio.API
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
            services.AddInfrastructureServices(Configuration.GetConnectionString("DefaultConnection"));
            services.AddCors();
            services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddQueryType(d => d.Name("Queries"))
                .AddType<WorkQueries>()
                .AddType<TagQueries>()
                .AddType<AuthorQueries>()
                .AddMutationType(d => d.Name("Mutations"))
                .AddType<WorkMutations>()
                .AddType<TagMutations>()
                .AddType<AuthorMutations>()
                .Create());
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

            app.UsePlayground();
            app.UseGraphQL("/graphql");
        }
    }
}
