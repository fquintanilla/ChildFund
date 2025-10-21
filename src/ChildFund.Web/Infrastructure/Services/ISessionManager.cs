using ChildFund.Services.Models;

namespace ChildFund.Web.Infrastructure.Services;

public interface ISessionManager
{
    TransactionInfoDto? CurrentTransaction { get; set; }
    bool ImpersonatedUser { get; set; }
    bool IsAuthenticated { get; set; }

    // Generic helpers
    T? Get<T>(string key);
    void Set<T>(string key, T value);
    void Remove(string key);
    void Clear();
}
