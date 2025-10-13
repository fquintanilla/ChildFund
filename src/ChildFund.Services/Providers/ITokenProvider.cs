using System.Net.Http.Headers;

namespace ChildFund.Services.Providers;

public interface ITokenProvider
{
    Task<string> GetTokenAsync(CancellationToken ct = default);
    Task<AuthenticationHeaderValue> GetAuthHeaderAsync(CancellationToken ct = default);
}

