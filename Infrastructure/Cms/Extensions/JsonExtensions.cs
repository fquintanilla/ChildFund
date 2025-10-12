using Newtonsoft.Json;
using System.Reflection;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class JsonExtensions
{
    public static string GetJsonPropertyName(Type type, string propertyName)
    {
        var currentProperty = type.GetProperties().FirstOrDefault(p => p.Name == propertyName);
        if (currentProperty != null)
        {
            return currentProperty.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? string.Empty;
        }

        return string.Empty;
    }

    public static List<(string name, Type type)> PropertyNames(Type type) =>
        type.GetProperties().Select(prop => (prop.Name, prop.PropertyType)).ToList();
}