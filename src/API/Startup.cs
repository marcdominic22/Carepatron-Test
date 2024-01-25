using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Application;
using Application.Common.Converters;
using Application.Common.Models;
using API.Converters;
using API.Filters;
using API.Swagger;

using FluentValidation.AspNetCore;

using Infrastructure;

using Newtonsoft.Json;
using Application.Common.ModelBinderProviders;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Serialization;

namespace API
{
    public class Startup
    {
        private readonly string PermittedRequestOrigins = "_permittedRequestOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddHealthChecks();
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddHttpContextAccessor();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                });

            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
                options.Filters.Add<ApiExceptionFilterAttribute>();

                options.ModelBinderProviders.Insert(0, new NullModelBinderProvider());
            })
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new NumberConverter());
                options.SerializerSettings.Converters.Add(new DecimalConverter());
                options.SerializerSettings.Converters.Add(new BooleanConverter());
                options.SerializerSettings.Converters.Add(new WhiteSpaceConverter());
            }).AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: PermittedRequestOrigins,
                    builder =>
                    {
                        builder.WithOrigins("*")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                    }
                );
            });

            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/v1/swagger.json", "API v1");
                options.DefaultModelsExpandDepth(-1);
                options.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(PermittedRequestOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                         .RequireCors(PermittedRequestOrigins);
            });

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse
                    {
                        GitVersion = Configuration.GetValue<string>("GitCommit") ?? "No Commit",
                        HealthCheckDuration = report.TotalDuration
                    };

                    var entries = new Dictionary<string, string>();

                    foreach (var entry in report.Entries)
                    {
                        entries.Add(entry.Key, entry.Value.Status.ToString());
                    }

                    response.Entries = entries;

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });
        }
    }
}
