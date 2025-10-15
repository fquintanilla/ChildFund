namespace ChildFund.Services.Interfaces;

/// <summary>
/// Provides throttling context information (client IP and client ID) for API requests.
/// This abstraction allows the Web layer to inject HTTP-specific context into the Services layer.
/// </summary>
public interface IThrottlingContextProvider
{
    /// <summary>
    /// Gets the client IP address for throttling purposes.
    /// Priority: True-Client-IP -> X-Forwarded-For -> RemoteIpAddress
    /// </summary>
    string? GetClientIp();

    /// <summary>
    /// Gets the client ID for throttling purposes.
    /// Priority: optimizely-client-id header -> Session ID
    /// </summary>
    string? GetClientId();
}

