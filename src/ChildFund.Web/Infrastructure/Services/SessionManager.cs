using ChildFund.Services.Models;

namespace ChildFund.Web.Infrastructure.Services;

public class SessionManager : ISessionManager
{
    private readonly IHttpContextAccessor _accessor;

    private ISession Session => _accessor.HttpContext?.Session
        ?? throw new InvalidOperationException("No HttpContext or Session available. Did you call app.UseSession()?");

    public SessionManager(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public TransactionInfoDto? CurrentTransaction
    {
        get => Session.GetObject<TransactionInfoDto>(SessionKeys.Transaction);
        set
        {
            if (value is null) Session.Remove(SessionKeys.Transaction);
            else Session.SetObject(SessionKeys.Transaction, value);
        }
    }

    public bool ImpersonatedUser
    {
        get => Session.GetInt32(SessionKeys.ImpersonatedUser) == 1;
        set => Session.SetInt32(SessionKeys.ImpersonatedUser, value ? 1 : 0);
    }

    public bool IsAuthenticated
    {
        get => Session.GetInt32(SessionKeys.IsAuthenticated) == 1;
        set => Session.SetInt32(SessionKeys.IsAuthenticated, value ? 1 : 0);
    }

    public T? Get<T>(string key)
    {
        // Handle primitive types with built-in session methods
        if (typeof(T) == typeof(string))
            return (T?)(object?)Session.GetString(key);

        if (typeof(T) == typeof(int) || typeof(T) == typeof(int?))
            return (T?)(object?)Session.GetInt32(key);

        if (typeof(T) == typeof(bool) || typeof(T) == typeof(bool?))
            return (T?)(object?)(Session.GetInt32(key) == 1);

        // Handle complex objects with JSON serialization
        return Session.GetObject<T>(key);
    }

    public void Set<T>(string key, T value)
    {
        // Use built-in session methods for primitive types
        if (value is string s)
        {
            Session.SetString(key, s);
        }
        else if (value is int i)
        {
            Session.SetInt32(key, i);
        }
        else if (value is bool b)
        {
            Session.SetInt32(key, b ? 1 : 0);
        }
        else if (value is null)
        {
            Session.Remove(key);
        }
        else
        {
            // Use JSON serialization for complex objects
            Session.SetObject(key, value);
        }
    }

    public void Remove(string key) => Session.Remove(key);
    public void Clear() => Session.Clear();
}
