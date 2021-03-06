﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCorePoc.Application.Mapper;
using NetCorePoc.Infrastructure.CrossCutting.Security;

namespace NetCorePoc.Api
{
    public partial class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            Infrastructure.CrossCutting.IoC.Domain.GetInstance.Initiate(services, Configuration).Load();
            services.Configure<TokenAuthSettings>(options => Configuration.GetSection("TokenAuthSettings").Bind(options));
            ConfigureServiceAuth(services);

            services.AddMvc();

            ConfigureServicesSwagger(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureJwtTokenAuthentication(app);

            AutoMapperApp.ConfigureAutoMapper();

            loggerFactory.AddFile("Logs/NetCore-{Date}.txt");

            ConfigureSwagger(app);

            app.UseMvc();
        }
    }
}
