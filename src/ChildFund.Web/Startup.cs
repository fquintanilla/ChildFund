using Advanced.CMS.GroupingHeader;
using Baaijte.Optimizely.ImageSharp.Web;
using ChildFund.Services.Extensions;
using ChildFund.Web.Core.CustomRoutes.Error;
using ChildFund.Web.Infrastructure;
using ChildFund.Web.Infrastructure.Cms.Helpers;
using ChildFund.Web.Infrastructure.Cms.Users;
using ChildFund.Web.Infrastructure.Commerce.Extensions;
using ChildFund.Web.Infrastructure.Display;
using ChildFund.Web.Infrastructure.Initialization;
using ChildFund.Web.Infrastructure.Middlewares;
using ChildFund.Web.Infrastructure.Rendering;
using EPiServer.Cms.TinyMce;
using EPiServer.Marketing.Testing.Web.Initializers;
using Geta.Optimizely.Categories.Configuration;
using Geta.Optimizely.Categories.Infrastructure.Initialization;
using Geta.Optimizely.ContentTypeIcons.Infrastructure.Configuration;
using Geta.Optimizely.ContentTypeIcons.Infrastructure.Initialization;
using Mediachase.Commerce.Anonymous;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Security;
using System.Text.Json.Serialization;
using UNRVLD.ODP.VisitorGroups.Initilization;

namespace ChildFund.Web;

public class Startup(
    IWebHostEnvironment webHostingEnvironment,
    IConfiguration configuration)
{
    private readonly long _maxRequestSize = 419_430_400; //400MB

    public void ConfigureServices(IServiceCollection services)
    {
        if (webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        //Connection strings
        var connectionString = configuration.GetConnectionString("EPiServerDB");
        var commerceConnectionString = configuration.GetConnectionString("EcfSqlConnection");
        services.Configure<DataAccessOptions>(options =>
        {
            options.ConnectionStrings.Add(new ConnectionStringOptions { Name = "EPiServerDB", ConnectionString = connectionString });
            options.ConnectionStrings.Add(new ConnectionStringOptions { Name = "EcfSqlConnection", ConnectionString = commerceConnectionString });
        });

        //AB Testing plugin
        services.AddABTesting(connectionString);

        //Allow uploading large files to the CMS
        services.Configure<FormOptions>(x =>
        {
            x.MultipartBodyLengthLimit = _maxRequestSize;
            x.ValueLengthLimit = int.MaxValue;
            x.MultipartBoundaryLengthLimit = int.MaxValue;
            x.MultipartHeadersCountLimit = int.MaxValue;
            x.MultipartHeadersLengthLimit = int.MaxValue;
        });

        services.Configure<IISServerOptions>(options =>
        {
            options.MaxRequestBodySize = _maxRequestSize;
        });
        services.Configure<KestrelServerOptions>(options =>
        {
            options.Limits.MaxRequestBodySize = _maxRequestSize;
            options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
        });

        // Adds 1-year public cache control to all media files
        // TODO: Uncomment this before going to prod
        /*services.Configure<MediaOptions>(options =>
		{
			options.CacheControl = "public,max-age=31536000";
			options.ExpirationTime = TimeSpan.FromDays(365);
		});*/

        //Disabling this as it is not needed and causes http 500 errors
        services.Configure<UIOptions>(options =>
        {
            options.WebSocketEnabled = false;
        });

        services.AddSession();
        services.AddCms();
        services.AddDisplay();
        services.AddTinyMce();

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

        services.AddDetection();
        services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
        services.AddControllers()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            )
            .AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });

        services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
            .AddRazorOptions(ro => ro.ViewLocationExpanders.Add(new FeatureViewLocationExpander()));

        services.AddChildFundServices(configuration);

        //Commerce
        services.AddCommerce();

        services.AddAdminUserRegistration(opt =>
            {
                opt.Behavior = EPiServer.Cms.Shell.UI.RegisterAdminUserBehaviors.Enabled;
            });

        services.AddEmbeddedLocalization<Startup>();
        services.AddTinyMceConfiguration();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddGetaCategories();
        services.TryAddEnumerable(Microsoft.Extensions.DependencyInjection.ServiceDescriptor.Singleton(typeof(IFirstRequestInitializer), typeof(ContentInstaller)));

        //Lowercase urls, remove trailing slash
        services.Configure<RouteOptions>(o =>
        {
            o.LowercaseUrls = true;
            o.AppendTrailingSlash = false;
        });

        //Image Sharp
        services.AddBaaijteOptimizelyImageSharp();
        // TODO: Fix this when setting up integration
        /*if (!EnvironmentHelper.IsLocal())
        {
	        services.AddCmsCloudPlatformSupport(configuration);
	        services.AddCommerceCloudPlatformSupport(configuration);
	        services.AddImageSharp()
		        .Configure<AzureBlobStorageCacheOptions>(options =>
		        {
			        options.ConnectionString = _configuration.GetConnectionString("EPiServerAzureBlobs");
			        options.ContainerName = "mysitemedia";
		        })
		        .ClearProviders()
		        .AddProvider<BlobImageProvider>()
		        .SetCache<AzureBlobStorageCache>();
	        services.Configure<ClientResourceOptions>(uiOptions => uiOptions.Debug = false);
        }
        else
        {*/
        services.Configure<ClientResourceOptions>(uiOptions => uiOptions.Debug = true);
        //}

        //Geo Location
        services.AddMaxMindGeolocationProvider("assets/GeoLite2-Country.mmdb", "assets/GeoLite2-Country-Locations-en.csv");

        //Unsecure http client
        services.AddHttpClient("HttpClientWithSSLUntrusted").ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        if (EnvironmentHelper.IsLocal())
                        {
                            return true; // for development, trust all certificates
                        }

                        return policyErrors == SslPolicyErrors.None; // Compliant: trust only some certificates
                    }
            });

        //Content Type Icons
        services.AddContentTypeIcons(x =>
        {
            x.EnableTreeIcons = true;
            x.ForegroundColor = "#ffffff";
            x.BackgroundColor = "#2c5697";
            x.FontSize = 40;
            x.CachePath = "[appDataPath]\\thumb_cache\\";
            x.CustomFontPath = "[appDataPath]\\fonts\\";
        });

        // Optimizely ODP
        services.AddODPVisitorGroups();

        // Grouping Header
        services.AddGroupingHeader();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        //Limit Image Resizing
        app.UseMiddleware<RestrictImageResizeMiddleware>();

        app.UseMiddleware<ErrorHandlerMiddleware>();

        app.UseCookiePolicy(
            new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always,
                HttpOnly = HttpOnlyPolicy.Always
            });

        // Add the image processing middleware.
        app.UseBaaijteOptimizelyImageSharp();

        app.UseAnonymousId();
        app.UseSession();
        app.UseDetection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAnonymousCartMerging();

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapControllerRoute(name: "Default", pattern: "{controller}/{action}/{id?}");
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            endpoints.MapContent();
        });

        //Remove trailing slash from opti urls
        ServiceLocator.Current.GetInstance<RoutingOptions>().UseTrailingSlash = false;

        //Custom Rewrites
        var options = new RewriteOptions()
                //Remove trailing slash
                .AddRedirect(@"^(.*)/$", "$1", StatusCodes.Status301MovedPermanently);

        //Redirect to HTTPS on non local environments
        if (!EnvironmentHelper.IsLocal())
        {
            options = options.AddRedirectToHttpsPermanent();
        }

        app.UseRewriter(options);

        app.UseGetaCategories();

        app.UseContentTypeIcons();

        //Set default regex timeout
        AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(500));
    }
}
