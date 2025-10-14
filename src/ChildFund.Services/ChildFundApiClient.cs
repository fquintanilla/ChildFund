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
/// </remarks>
public abstract class ChildFundApiClient
{
    protected readonly HttpClient Http;
    private readonly ITokenProvider _tokenProvider;

    protected ChildFundApiClient(
        HttpClient http,
        ITokenProvider tokenProvider,
        IOptions<ChildFundApiOptions> options)
    {
        Http = http ?? throw new ArgumentNullException(nameof(http));
        _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));

        if (options?.Value == null)
            throw new ArgumentNullException(nameof(options));

        Http.BaseAddress = new Uri(options.Value.BaseUrl.TrimEnd('/') + "/");
    }

    /// <summary>
    /// Performs an authenticated GET request and deserializes the response.
    /// </summary>
    protected async Task<T> GetAsync<T>(
        string relativePath,
        JsonSerializerOptions? jsonOptions = null,
        CancellationToken ct = default)
    {
        await EnsureAuthAsync(ct);
        
        using var resp = await Http.GetAsync(relativePath, ct);
        resp.EnsureSuccessStatusCode();
        
        await using var stream = await resp.Content.ReadAsStreamAsync(ct);
        return (await JsonSerializer.DeserializeAsync<T>(stream, jsonOptions ?? JsonDefaults.Options, ct))!;
    }

    /// <summary>
    /// Performs an authenticated POST request and deserializes the response.
    /// </summary>
    protected async Task<T> PostAsync<T>(
        string relativePath,
        object? body = null,
        JsonSerializerOptions? jsonOptions = null,
        CancellationToken ct = default)
    {
        await EnsureAuthAsync(ct);

        using var content = body is null
            ? null
            : new StringContent(
                JsonSerializer.Serialize(body, jsonOptions ?? JsonDefaults.Options),
                System.Text.Encoding.UTF8,
                "application/json");

        using var resp = await Http.PostAsync(relativePath, content, ct);
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
    /// Default retry policy for transient HTTP errors (network failures, 5xx, 429).
    /// Uses exponential backoff: 200ms, 400ms, 800ms.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> DefaultRetryPolicy() =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => (int)msg.StatusCode == 429)
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
}

