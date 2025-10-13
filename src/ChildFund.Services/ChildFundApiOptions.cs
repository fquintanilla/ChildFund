namespace ChildFund.Services;

public sealed class ChildFundApiOptions
{
    /// <summary>Base URL, e.g. "https://pubwebapi.childfund.org/api/v1"</summary>
    public string BaseUrl { get; set; } = null!;

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
}

