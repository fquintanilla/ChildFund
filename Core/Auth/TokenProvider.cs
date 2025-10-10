using System.Net.Http.Headers;
using System.Text.Json;
using ChildFund.Core.Http;
using ChildFund.Core.Options;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Options;

namespace ChildFund.Core.Auth
{
    public interface ITokenProvider
    {
        Task<string> GetTokenAsync(CancellationToken ct = default);
        Task<AuthenticationHeaderValue> GetAuthHeaderAsync(CancellationToken ct = default);
    }

    [ServiceConfiguration(typeof(ITokenProvider))]
    internal sealed class TokenProvider(
        IHttpClientFactory httpClientFactory,
        IOptions<ChildFundOptions> options)
        : ITokenProvider
    {
        private const string CacheKey = "ChildFund.AuthToken";

        public async Task<string> GetTokenAsync(CancellationToken ct = default)
        {
            var cached = CacheManager.Get(CacheKey) as string;
            if (!string.IsNullOrEmpty(cached))
            {
                return cached;
            }

            var client = httpClientFactory.CreateClient("childfund-auth");

            var content = new StringContent($"User={options.Value.ApiKey}");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            using var resp = await client.PostAsync(options.Value.AuthenticatePath, content, ct);
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

            var eviction = new CacheEvictionPolicy(ttl, CacheTimeoutType.Absolute);
            CacheManager.Insert(CacheKey, auth.Token, eviction);

            return auth.Token;
        }

        public async Task<AuthenticationHeaderValue> GetAuthHeaderAsync(CancellationToken ct = default) =>
            new("Bearer", await GetTokenAsync(ct));
    }
}
