using System.Text.Json;
using JOS.ApiKeyAuthentication.Web.Features.Authentication;
using JOS.ApiKeyAuthentication.Web.Features.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace JOS.ApiKeyAuthentication.Web
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options => {});

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.OnlyEmployees, policy => policy.Requirements.Add(new OnlyEmployeesRequirement()));
                options.AddPolicy(Policies.OnlyManagers, policy => policy.Requirements.Add(new OnlyManagersRequirement()));
                options.AddPolicy(Policies.OnlyThirdParties, policy => policy.Requirements.Add(new OnlyThirdPartiesRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, OnlyEmployeesAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, OnlyManagersAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, OnlyThirdPartiesAuthorizationHandler>();

            services.AddSingleton<IGetApiKeyQuery, InMemoryGetApiKeyQuery>();

            services.AddRouting();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
