using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.DataAccess.Extension;
using JoggingTracker.Service.Infrastructure.PipelineBehavior;
using JoggingTracker.Shared.Configuration;
using JoggingTracker.WebApi.Infrastructure.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;

namespace JoggingTracker.WebApi
{
    public partial class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // Read the configuration keys from the secret store.
                builder.AddUserSecrets<Startup>();
            }

            this.Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            this.ConfigureOptions(services);

            services.AddEntityFramework(this.Configuration);

            services.AddCors();

            services.AddMvc(config =>
            {
                // Require authenticated user to access any controller action by default.
                // Anonymous access must be declared explicitly on controller method.
                var policy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
                config.RespectBrowserAcceptHeader = true;
            });

            services.AddAutoMapper();

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info
                {
                    Title = "Jogging Tracker API",
                    Version = "v1"
                });

                var applicationPath = PlatformServices.Default.Application.ApplicationBasePath;
                config.IncludeXmlComments(Path.Combine(applicationPath, "WebApi.xml"));
            });

            services.AddMediatR(typeof(ValidatorBehavior<,>).GetTypeInfo().Assembly);

            return AutofacInitializer.Initialize(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection(Logging.SectionName));
            loggerFactory.AddDebug();

            this.ConfigureExceptionHandler(app, env);
            this.ConfigureAuth(app);

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());

            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.RoutePrefix = "api/documentation";
                config.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    description: "Jogging Tracker API v1");
            });

            DatabaseInitializer.Initialize(app.ApplicationServices, env.IsDevelopment());
        }
    }
}
