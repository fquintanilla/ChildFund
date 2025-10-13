using EPiServer.Framework.Cache;

namespace ChildFund.Infrastructure.Cms.Services;

public interface ICacheService
{
	void Add<T>(string key, T obj);
	void Add<T>(string key, T obj, TimeSpan slidingExpiration);
	void AddByDays<T>(string key, T obj, int days);
	void AddBySeconds<T>(string key, T obj, int seconds);
	bool Exists(string key);
	T Get<T>(string key);
	void Remove(string key);
}

public class CacheService : ICacheService
{
	private readonly ISynchronizedObjectInstanceCache _cache;

	public CacheService(ISynchronizedObjectInstanceCache cache) => _cache = cache;

	public void Add<T>(string key, T obj)
	{
		if (obj != null && !string.IsNullOrEmpty(key))
		{
			_cache.Insert(key, obj, CacheEvictionPolicy.Empty);
		}
	}

	public void Add<T>(string key, T obj, TimeSpan slidingExpiration)
	{
		if (obj != null && !string.IsNullOrEmpty(key))
		{
			_cache.Insert(key, obj, new CacheEvictionPolicy(
				slidingExpiration, CacheTimeoutType.Sliding));
		}
	}

	public void AddBySeconds<T>(string key, T obj, int seconds)
	{
		if (obj != null && !string.IsNullOrEmpty(key))
		{
			_cache.Insert(key, obj, new CacheEvictionPolicy(
				TimeSpan.FromSeconds(seconds), CacheTimeoutType.Sliding));
		}
	}

	public void AddByDays<T>(string key, T obj, int days)
	{
		if (obj != null && !string.IsNullOrEmpty(key))
		{
			_cache.Insert(key, obj, new CacheEvictionPolicy(
				TimeSpan.FromDays(days), CacheTimeoutType.Sliding));
		}
	}

	public T Get<T>(string key)
	{
		try
		{
			return (T)_cache.Get(key);
		}
		catch
		{
			return default;
		}
	}

	public void Remove(string key)
	{
		if (!string.IsNullOrEmpty(key))
		{
			_cache.Remove(key);
		}
	}

	public bool Exists(string key)
	{
		if (!string.IsNullOrEmpty(key))
		{
			return _cache.Get(key) != null;
		}

		return false;
	}
}