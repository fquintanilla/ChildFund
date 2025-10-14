using ChildFund.Services.ApiClients;
using ChildFund.Services.Interfaces;
using ChildFund.Services.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;

namespace ChildFund.Services.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all ChildFund API services and configures HttpClientFactory with proper
    /// connection pooling, timeouts, TCP keepalive, and service point settings.
    /// </summary>
    public static IServiceCollection AddChildFundServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register configuration
        services.Configure<ChildFundApiOptions>(configuration.GetSection("ChildFund"));

        // Register memory cache if not already registered
        services.AddMemoryCache();

        // Register TokenProvider as singleton for token caching
        services.AddSingleton<ITokenProvider, TokenProvider>();

        // Build service provider to get options for HttpMessageHandler configuration
        var serviceProvider = services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<ChildFundApiOptions>>().Value;

        // Configure HTTP client for authentication
        services.AddHttpClient("childfund-auth", (sp, client) =>
            {
                ConfigureHttpClient(client, options);
            })
            .ConfigurePrimaryHttpMessageHandler(() => CreateHttpMessageHandler(options))
            .AddPolicyHandler(ChildFundApiClient.DefaultRetryPolicy());

        // Configure HTTP client for API calls
        services.AddHttpClient<IChildInventoryClient, ChildInventoryClient>((sp, client) =>
            {
                ConfigureHttpClient(client, options);
            })
            .ConfigurePrimaryHttpMessageHandler(() => CreateHttpMessageHandler(options))
            .AddPolicyHandler(ChildFundApiClient.DefaultRetryPolicy());

        services.AddHttpClient<IDonorPortalClient, DonorPortalClient>((sp, client) =>
            {
                ConfigureHttpClient(client, options);
            })
            .ConfigurePrimaryHttpMessageHandler(() => CreateHttpMessageHandler(options))
            .AddPolicyHandler(ChildFundApiClient.DefaultRetryPolicy());

        return services;
    }

    /// <summary>
    /// Configures common HTTP client settings including base address, timeout, and headers.
    /// </summary>
    private static void ConfigureHttpClient(HttpClient client, ChildFundApiOptions options)
    {
        client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
        client.Timeout = TimeSpan.FromSeconds(options.RequestTimeoutSeconds);
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        client.DefaultRequestHeaders.Connection.ParseAdd("keep-alive");
    }

    /// <summary>
    /// Creates an HttpMessageHandler with optimized settings for connection pooling,
    /// TCP keepalive, and service point configuration.
    /// </summary>
    private static HttpMessageHandler CreateHttpMessageHandler(ChildFundApiOptions options)
    {
        // Enable HTTP/2 and configure ServicePoint settings for optimal performance
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        ServicePointManager.DefaultConnectionLimit = 100;
        ServicePointManager.Expect100Continue = false;
        ServicePointManager.UseNagleAlgorithm = false;

        var handler = new SocketsHttpHandler
        {
            // Connection pooling and lifetime settings
            PooledConnectionLifetime = TimeSpan.FromMinutes(options.ConnectionLifetimeMinutes),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(options.ConnectionIdleTimeoutMinutes),
            MaxConnectionsPerServer = options.MaxConnectionsPerServer,

            // HTTP/2 settings
            EnableMultipleHttp2Connections = true,

            // DNS settings
            UseCookies = false, // Cookies are typically handled at application level

            // Response settings
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        };

        return handler;
    }

    /// <summary>
    /// Alternative configuration method that allows customizing options via a delegate.
    /// </summary>
    public static IServiceCollection AddChildFundServices(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<ChildFundApiOptions> configureOptions)
    {
        services.AddChildFundServices(configuration);
        services.Configure(configureOptions);
        return services;
    }
}

