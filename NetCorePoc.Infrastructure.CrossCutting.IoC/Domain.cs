using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NetCorePoc.Application.Apps;
using NetCorePoc.Application.Interfaces;
using NetCorePoc.Domain.Interfaces.Repositories;
using NetCorePoc.Domain.Interfaces.UnitOfWork;
using NetCorePoc.Infrastructure.CrossCutting.DataAccess.Context;
using NetCorePoc.Infrastructure.CrossCutting.DataAccess.Repositories;
using NetCorePoc.Infrastructure.CrossCutting.DataAccess.UnitOfWork;

namespace NetCorePoc.Infrastructure.CrossCutting.IoC
{
    public partial class Domain : IDomain
    {
        private static Domain _instance;
        private IServiceCollection _services;
        private IConfiguration _configuration;

        public static Domain GetInstance => _instance ?? (_instance = new Domain());

        public Domain Initiate(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;

            return this;
        }

        public void Load()
        {
            ConfigureServices();            
        }

        public IServiceProvider GetServiceProvider()
        {
            return _services.BuildServiceProvider();
        }

        private void ConfigureServices()
        {
            _services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            _services.AddDbContext<DomainContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            _services.AddTransient<IUnitOfWork, UnitOfWork>();

            _services.AddTransient<IUserAppService, UserAppService>();

            _services.AddTransient<IUserRepository, UserRepository>();
        }

    }
}