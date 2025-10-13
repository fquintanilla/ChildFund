namespace ChildFund.Infrastructure.Cms.Services;

public interface IConfigurationService
{
	string GetAppSetting(string key);
	T GetAppSetting<T>(string key, T defaultValue);
	string GetConnectionString(string name);
}

public class ConfigurationService : IConfigurationService
{
	private readonly IConfiguration _configuration;

	public ConfigurationService(IConfiguration configuration) => _configuration = configuration;

	public T GetAppSetting<T>(string key, T defaultValue)
	{
		if (!string.IsNullOrEmpty(key))
		{
			var value = _configuration[key];

			if (value == null)
			{
				return defaultValue;
			}

			var itemType = typeof(T);
			if (itemType.IsEnum)
			{
				return (T)Enum.Parse(itemType, Convert.ToString(value), true);
			}

			return (T)Convert.ChangeType(value, itemType);
		}

		return defaultValue;
	}

	public string GetAppSetting(string key) => GetAppSetting(key, string.Empty);

	public string GetConnectionString(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return null;
		}

		var entry = _configuration[$"ConnectionStrings:{name}"];
		return entry;
	}
}