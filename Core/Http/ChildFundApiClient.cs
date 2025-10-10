using ChildFund.Core.Auth;
using ChildFund.Core.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Polly;
using Polly.Extensions.Http;

namespace ChildFund.Core.Http
{
    public abstract class ChildFundApiClient
    {
        protected readonly HttpClient Http;
        private readonly ITokenProvider _tokenProvider;

        protected ChildFundApiClient(HttpClient http, ITokenProvider tokenProvider, IOptions<ChildFundOptions> options)
        {
            Http = http;
            _tokenProvider = tokenProvider;
            Http.BaseAddress = new Uri(options.Value.BaseUrl.TrimEnd('/') + "/");
        }

        protected async Task<T> GetAsync<T>(string relativePath, JsonSerializerOptions? jsonOptions = null, CancellationToken ct = default)
        {
            await EnsureAuthAsync(ct);
            using var resp = await Http.GetAsync(relativePath, ct);
            resp.EnsureSuccessStatusCode();
            var stream = await resp.Content.ReadAsStreamAsync(ct);
            return (await JsonSerializer.DeserializeAsync<T>(stream, jsonOptions ?? JsonDefaults.Options, ct))!;
        }

        private async Task EnsureAuthAsync(CancellationToken ct)
        {
            var header = await _tokenProvider.GetAuthHeaderAsync(ct);
            if (Http.DefaultRequestHeaders.Authorization?.Parameter != header.Parameter)
                Http.DefaultRequestHeaders.Authorization = header;
        }

        public static IAsyncPolicy<HttpResponseMessage> DefaultRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode == 429)
                .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt)));
    }

}
