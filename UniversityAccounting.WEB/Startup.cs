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
using UniversityAccounting.WEB.Models.HelperClasses;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.Extensions.Options;
using UniversityAccounting.WEB.Controllers.HelperClasses;

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

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IBreadcrumbNodeCreator, BreadcrumbNodeCreator>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru"),
                    new CultureInfo("ar")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization()
                .AddMvcOptions(opts => opts.ModelBindingMessageProvider
                    .SetAttemptedValueIsInvalidAccessor((value, prop) =>
                        string.Format(Resources.Startup.ValueIsInvalidMessage, value, prop)));

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddBreadcrumbs(GetType().Assembly, options => options.DontLookForDefaultNode = true);
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
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions?.Value);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "studentPaging",
                    pattern: "Students/groupId-{groupId}/Page{page}",
                    defaults: new {Controller = "Students", action = "Index"});
                endpoints.MapControllerRoute(
                    name: "groupPaging",
                    pattern: "Groups/courseId-{courseId}/Page{page}",
                    defaults: new {Controller = "Groups", action = "Index"});
                endpoints.MapControllerRoute(
                    name: "coursePaging",
                    pattern: "Courses/Page{page}",
                    defaults: new {Controller = "Courses", action = "Index"});
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Courses}/{action=Index}/{id?}");
            });

            SeedData.InitializeData(app);
            app.UseNotyf();
        }
    }
}
