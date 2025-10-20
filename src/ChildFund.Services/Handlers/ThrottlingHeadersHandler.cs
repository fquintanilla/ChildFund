using ChildFund.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChildFund.Services.Handlers;

/// <summary>
/// HTTP message handler that adds throttling headers to API requests.
/// Headers help the API identify users by IP and session to prevent abuse.
/// </summary>
public class ThrottlingHeadersHandler : DelegatingHandler
{
    private readonly IThrottlingContextProvider? _contextProvider;
    private readonly ILogger<ThrottlingHeadersHandler> _logger;
    private readonly bool _enabled;

    public ThrottlingHeadersHandler(
        ILogger<ThrottlingHeadersHandler> logger,
        IOptions<ChildFundApiOptions> options,
        IThrottlingContextProvider? contextProvider = null)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _contextProvider = contextProvider;
        _enabled = options?.Value?.EnableThrottlingHeaders ?? false;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_enabled && _contextProvider != null)
        {
            AddThrottlingHeaders(request);
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private void AddThrottlingHeaders(HttpRequestMessage request)
    {
        try
        {
            // Add Client IP Header
            var clientIp = _contextProvider!.GetClientIp();
            if (!string.IsNullOrWhiteSpace(clientIp))
            {
                _logger.LogInformation("Setting Optimizely_Client_IP to {ClientIp}", clientIp);
                request.Headers.Remove("Optimizely_Client_IP");
                request.Headers.Add("Optimizely_Client_IP", clientIp);
            }

            // Add Client ID Header
            var clientId = _contextProvider.GetClientId();
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                _logger.LogInformation("Setting Optimizely-Client-ID to {ClientId}", clientId);
                request.Headers.Remove("Optimizely-Client-ID");
                request.Headers.Add("Optimizely-Client-ID", clientId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding throttling headers to API request");
        }
    }
}


