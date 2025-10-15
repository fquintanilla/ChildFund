using ChildFund.Services.Providers;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Text.Json;

namespace ChildFund.Services;

/// <summary>
/// Base class for all ChildFund API clients. Provides common functionality for HTTP requests
/// with authentication, retry policies, and proper resource disposal.
/// </summary>
/// <remarks>
/// HttpClient is managed by IHttpClientFactory and should NOT be disposed by derived classes.
/// The factory handles proper connection pooling, lifetime management, and disposal.
/// Supports both synchronous and asynchronous API endpoints based on configuration.
/// </remarks>
public abstract class ChildFundApiClient
{
    protected readonly HttpClient Http;
    private readonly ITokenProvider _tokenProvider;
    private readonly bool _useAsyncEndpoints;

    protected ChildFundApiClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
    {
        Http = http ?? throw new ArgumentNullException(nameof(http));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

        if (options?.Value == null)
            throw new ArgumentNullException(nameof(options));

        var opts = options.Value;
        _useAsyncEndpoints = opts.UseAsyncEndpoints;
        Http.BaseAddress = new Uri(opts.EffectiveBaseUrl.TrimEnd('/') + "/");
    }

    /// <summary>
    /// Performs an authenticated GET request and deserializes the response.
    /// Automatically appends "Async" to the path when UseAsyncEndpoints is enabled.
    /// </summary>
    protected async Task<T> GetAsync<T>(
        string relativePath,
        JsonSerializerOptions? jsonOptions = null,
        CancellationToken ct = default)
    {
        await EnsureAuthAsync(ct);

        var effectivePath = _useAsyncEndpoints ? AppendAsyncToPath(relativePath) : relativePath;
        using var resp = await Http.GetAsync(effectivePath, ct);
        resp.EnsureSuccessStatusCode();

        await using var stream = await resp.Content.ReadAsStreamAsync(ct);
        return (await JsonSerializer.DeserializeAsync<T>(stream, jsonOptions ?? JsonDefaults.Options, ct))!;
    }

    /// <summary>
    /// Performs an authenticated POST request and deserializes the response.
    /// Automatically appends "Async" to the path when UseAsyncEndpoints is enabled.
    /// </summary>
    protected async Task<T> PostAsync<T>(
        string relativePath,
        object? body = null,
        JsonSerializerOptions? jsonOptions = null,
        CancellationToken ct = default)
    {
        await EnsureAuthAsync(ct);

        var effectivePath = _useAsyncEndpoints ? AppendAsyncToPath(relativePath) : relativePath;
        using var content = body is null
            ? null
            : new StringContent(
                JsonSerializer.Serialize(body, jsonOptions ?? JsonDefaults.Options),
                System.Text.Encoding.UTF8,
                "application/json");

        using var resp = await Http.PostAsync(effectivePath, content, ct);
        resp.EnsureSuccessStatusCode();

        await using var stream = await resp.Content.ReadAsStreamAsync(ct);

        return (await JsonSerializer.DeserializeAsync<T>(stream, jsonOptions ?? JsonDefaults.Options, ct))!;
    }

    private async Task EnsureAuthAsync(CancellationToken ct)
    {
        var header = await _tokenProvider.GetAuthHeaderAsync(ct);
        if (Http.DefaultRequestHeaders.Authorization?.Parameter != header.Parameter)
            Http.DefaultRequestHeaders.Authorization = header;
    }

    /// <summary>
    /// Appends "Async" to the last segment of the path.
    /// Example: "ChildInventory/GetRandomKidsForWeb" becomes "ChildInventory/GetRandomKidsForWebAsync"
    /// </summary>
    private static string AppendAsyncToPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;

        // Handle query strings
        var queryIndex = path.IndexOf('?');
        if (queryIndex >= 0)
        {
            var pathPart = path.Substring(0, queryIndex);
            var queryPart = path.Substring(queryIndex);
            return AppendAsyncToPath(pathPart) + queryPart;
        }

        // Find the last segment after the last slash
        var lastSlashIndex = path.LastIndexOf('/');
        if (lastSlashIndex >= 0 && lastSlashIndex < path.Length - 1)
        {
            var beforeMethod = path.Substring(0, lastSlashIndex + 1);
            var method = path.Substring(lastSlashIndex + 1);
            return beforeMethod + method + "Async";
        }

        // No slash found, just append to the entire path
        return path + "Async";
    }

    /// <summary>
    /// Default retry policy for transient HTTP errors (network failures, 5xx, 429).
    /// Uses exponential backoff: 200ms, 400ms, 800ms.
    /// Only retry once.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> DefaultRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => (int)msg.StatusCode == 429)
            .WaitAndRetryAsync(1, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
}
