using ChildFund.Infrastructure.Cms.Users;
using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Geta.Optimizely.Categories.Configuration;
using Geta.Optimizely.Categories.Infrastructure.Initialization;
using Mediachase.Commerce.Anonymous;

namespace ChildFund;

public class Startup(
    IWebHostEnvironment webHostingEnvironment,
    IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services)
    {
        if (webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services.Configure<DataAccessOptions>(options => options.ConnectionStrings.Add(new ConnectionStringOptions
        {
            Name = "EPiServerDB",
            ConnectionString = configuration.GetConnectionString("EPiServerDB")
        }));

        services.AddCmsAspNetIdentity<SiteUser>(o =>
        {
            if (string.IsNullOrEmpty(o.ConnectionStringOptions?.ConnectionString))
            {
                o.ConnectionStringOptions = new ConnectionStringOptions
                {
                    Name = "EPiServerDB",
                    ConnectionString = configuration.GetConnectionString("EPiServerDB")
                };
            }
        });

        services
            .AddCommerce()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>();

        services.AddGetaCategories();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseGetaCategories();

        app.UseAnonymousId();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(name: "Default", pattern: "{controller}/{action}/{id?}");
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapContent();
        });
    }
}
