using AutoMapper;
using dotimo.Business;
using dotimo.Business.Services;
using dotimo.Core;
using dotimo.Core.Repositories;
using dotimo.Data.Context;
using dotimo.Data.Entities;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace dotimo.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DotimoDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DotimoDbContext>();

            services.AddControllersWithViews();

            services.AddRazorPages();

            //IoC
            services.AddScoped(typeof(DbContext), typeof(DotimoDbContext));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<IUnitOfWork<Watch>, UnitOfWork<Watch>>();
            services.AddScoped<IWatchService, WatchService>();

            // Automapper
            MapperConfiguration mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new Mapping.MappingProfile()));
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            //hangfire
            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (IServiceScope serviceScope = app.ApplicationServices.CreateScope())
            {
                DotimoDbContext context = serviceScope.ServiceProvider.GetService<DotimoDbContext>();

                context.Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //hangfire
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Watches}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}