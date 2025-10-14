namespace ChildFund.Services;

public sealed class ChildFundApiOptions
{
    /// <summary>Base URL for synchronous endpoints, e.g. "https://pubwebapi.childfund.org/api/v1"</summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>Base URL for asynchronous endpoints, e.g. "https://pubwebapi.childfund.org/api/v1"</summary>
    public string AsyncBaseUrl { get; set; } = null!;

    /// <summary>If true, uses AsyncBaseUrl and appends "Async" to method names. Default is false.</summary>
    public bool UseAsyncEndpoints { get; set; } = false;

    /// <summary>Pre-encoded "User=..." payload expected by Authenticate endpoint.</summary>
    public string ApiKey { get; set; } = null!;

    /// <summary>Relative path to authenticate, default "Account/Authenticate"</summary>
    public string AuthenticatePath { get; set; } = "Account/Authenticate/";

    /// <summary>HTTP request timeout in seconds. Default is 30.</summary>
    public int RequestTimeoutSeconds { get; set; } = 30;

    /// <summary>Connection pool lifetime in minutes. Default is 5.</summary>
    public int ConnectionLifetimeMinutes { get; set; } = 5;

    /// <summary>Maximum number of connections per server. Default is 10.</summary>
    public int MaxConnectionsPerServer { get; set; } = 10;

    /// <summary>Connection idle timeout in minutes. Default is 2.</summary>
    public int ConnectionIdleTimeoutMinutes { get; set; } = 2;

    /// <summary>
    /// Gets the effective base URL based on UseAsyncEndpoints setting.
    /// </summary>
    public string EffectiveBaseUrl => UseAsyncEndpoints ? AsyncBaseUrl : BaseUrl;
}

