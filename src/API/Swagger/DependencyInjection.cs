using System;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Swagger
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // API XML Documentation
            var apiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
            var applicationXmlPath = $@"{AppContext.BaseDirectory}Application.xml";

            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Carepatron API",
                    Version = "v1",
                    Description = "API Documentation for Carepatron Test",
                    Contact = new OpenApiContact { Name = "Contact Dev Team" }
                });

                config.CustomSchemaIds(type => $"{type}");
                config.DescribeAllParametersInCamelCase();
                config.IncludeXmlComments(apiXmlPath);
                config.IncludeXmlComments(applicationXmlPath);
                // config.OperationFilter<ParameterFilter>();
               
            });

            return services;
        }
    }
}