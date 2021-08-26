using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartBreadcrumbs.Extensions;
using UniversityAccounting.DAL.EF;
using UniversityAccounting.DAL.Interfaces;
using UniversityAccounting.DAL.Repositories;

namespace UniversityAccounting.WEB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<UniversityContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(connection,
                        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddControllersWithViews();
            services.AddMvc();
            services.AddBreadcrumbs(GetType().Assembly);
            services.AddNotyf(config =>
            {
                config.DurationInSeconds = 5;
                config.IsDismissable = true;
                config.Position = NotyfPosition.TopCenter;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "studentPaging",
                    pattern: "Students/groupId-{groupId}/Page{page}",
                    defaults: new { Controller = "Students", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "groupPaging",
                    pattern: "Groups/courseId-{courseId}/Page{page}",
                    defaults: new { Controller = "Groups", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "coursePaging",
                    pattern: "Courses/Page{page}",
                    defaults: new { Controller = "Courses", action = "Index" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Courses}/{action=Index}/{id?}");
            });

            SeedData.InitializeData(app);
            app.UseNotyf();
        }
    }
}
