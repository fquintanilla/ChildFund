using ChildFund.Services.Models;
using ChildFund.Services.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ChildFund.Services.Providers;

internal sealed class TokenProvider : ITokenProvider
{
    private const string CacheKey = "ChildFund.AuthToken";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly ChildFundApiOptions _options;

    public TokenProvider(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IOptions<ChildFundApiOptions> options)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _options = options.Value;
    }

    public async Task<string> GetTokenAsync(CancellationToken ct = default)
    {
        if (_cache.TryGetValue<string>(CacheKey, out var cached) && !string.IsNullOrEmpty(cached))
        {
            return cached;
        }

        using var client = _httpClientFactory.CreateClient("childfund-auth");

        using var content = new StringContent($"User={_options.ApiKey}");
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        using var resp = await client.PostAsync(_options.AuthenticatePath, content, ct);
        resp.EnsureSuccessStatusCode();

        await using var s = await resp.Content.ReadAsStreamAsync(ct);
        var auth = await JsonSerializer.DeserializeAsync<AuthResponse>(s, JsonDefaults.Options, ct)
                   ?? throw new InvalidOperationException("Empty auth response.");

        if (string.IsNullOrWhiteSpace(auth.Token))
            throw new InvalidOperationException("Auth response missing token.");

        // Cache until slightly before the server's expiry to account for clock skew
        var now = DateTimeOffset.UtcNow;
        var safeExpiry = auth.ExpireDate - TimeSpan.FromMinutes(2);

        // Calculate remaining lifetime from now until safeExpiry
        var ttl = safeExpiry - now;

        if (safeExpiry <= now)
        {
            // Expiry already passed (or within 2 minutes): don't cache—just return.
            return auth.Token;
        }

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };

        _cache.Set(CacheKey, auth.Token, cacheOptions);

        return auth.Token;
    }

    public async Task<AuthenticationHeaderValue> GetAuthHeaderAsync(CancellationToken ct = default) =>
        new("Bearer", await GetTokenAsync(ct));
}

