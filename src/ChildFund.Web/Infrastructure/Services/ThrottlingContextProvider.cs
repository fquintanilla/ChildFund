using ChildFund.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace ChildFund.Web.Infrastructure.Services;

/// <summary>
/// Provides throttling context information from HTTP requests for API throttling.
/// Extracts client IP and client ID to help the API identify and throttle abusive users.
/// </summary>
public class ThrottlingContextProvider : IThrottlingContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ThrottlingContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    /// <inheritdoc />
    public string? GetClientIp()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request == null)
        {
            return null;
        }

        // 1. Check True-Client-IP header (set by CDNs like Akamai)
        if (httpContext.Request.Headers.TryGetValue("True-Client-IP", out var trueClientIp))
        {
            var ip = trueClientIp.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(ip) && IPAddress.TryParse(ip, out var parsedIp))
            {
                return parsedIp.ToString(); // Normalizes the IP format
            }
        }

        // 2. Check X-Forwarded-For header (set by proxies/load balancers)
        if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xForwardedFor))
        {
            var xff = xForwardedFor.ToString();
            if (!string.IsNullOrWhiteSpace(xff))
            {
                // Take the first IP in the chain (original client IP)
                var ip = xff.Split(',')[0].Trim();
                if (IPAddress.TryParse(ip, out var parsedIp))
                {
                    return parsedIp.ToString(); // Normalizes the IP format
                }
            }
        }

        // 3. Fallback to RemoteIpAddress
        var remoteIp = httpContext.Connection.RemoteIpAddress;
        if (remoteIp != null)
        {
            return remoteIp.ToString();
        }

        return null;
    }

    /// <inheritdoc />
    public string? GetClientId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request == null)
        {
            return null;
        }

        // 1. Check if client explicitly provided optimizely-client-id header
        if (httpContext.Request.Headers.TryGetValue("optimizely-client-id", out var clientIdHeader))
        {
            var clientId = clientIdHeader.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                return clientId;
            }
        }

        // 2. Fallback to session ID
        if (httpContext.Session != null)
        {
            var sessionId = httpContext.Session.Id;
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                return sessionId;
            }
        }

        return null;
    }
}

