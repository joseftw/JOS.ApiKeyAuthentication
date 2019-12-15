using System.Collections.Generic;
using JOS.ApiKeyAuthentication.Web.Features.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace JOS.ApiKeyAuthentication.Web.Features.Swagger
{
    public static class SwaggerConfigurator
    {
        public static void ConfigureSwaggerFeature(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "JOS.ApiKeyAuthentication", Version = "v1" });

                c.AddSecurityDefinition(ApiKeyConstants.HeaderName, new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                    In = ParameterLocation.Header,
                    Name = ApiKeyConstants.HeaderName,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme 
                        {
                            Name = ApiKeyConstants.HeaderName,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = ApiKeyConstants.HeaderName },
                        },
                        new string[] {}
                    }
                });
            });
        }
    }
}
