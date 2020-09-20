using dotimo.Business;
using dotimo.Business.IServices;
using dotimo.Business.Services;
using dotimo.Core;
using dotimo.Core.Repositories;
using dotimo.Data.Context;
using dotimo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace dotimo.Application
{
    public static class DependencyRegistrar
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped(typeof(DbContext), typeof(DotimoDbContext));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));

            services.AddScoped<INotificationService, EmailService>();
            services.AddScoped<IHangfireService, HangfireService>();
            services.AddScoped<IUnitOfWork<Watch>, UnitOfWork<Watch>>();
            services.AddScoped<IWatchService, WatchService>();
            services.AddScoped<IUnitOfWork<CheckUp>, UnitOfWork<CheckUp>>();
            services.AddScoped<ICheckUpService, CheckUpService>();
        }
    }
}