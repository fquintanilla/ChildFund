using Newtonsoft.Json;

namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class ViewExtensions
{
    private static readonly Injected<IContextModeResolver> _contextModeResolver;

    public static string Serialize<T>(this T input) where T : IContent =>
        JsonConvert.SerializeObject(input, Formatting.None);

    public static T Deserialize<T>(this string input)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<T>(input);
            return result;
        }
        catch
        {
            return default;
        }
    }

    public static JsonResult SendJsonHttpResponse<T>(this T data) where T : IContent
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.None
        };

        var msg = new JsonResult(data, settings);

        return msg;
    }

    public static ActionResult SendJsonMvcResponse<T>(this T data) where T : IContent
    {
        var result = new ContentResult { Content = data.Serialize(), ContentType = "text/json" };
        return result;
    }

    public static bool IsContentAreaNonEmpty(this ContentArea property) =>
        property?.Items != null && property.Items.Any();

    public static bool ValidateProperty(this XhtmlString property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit || property is { IsEmpty: false };

    public static bool ValidateProperty(this string property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit || !string.IsNullOrWhiteSpace(property);

    public static bool ValidateProperty(this ContentArea property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit || property.IsContentAreaNonEmpty();

    public static bool ValidateProperty(this LinkItem property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit ||
        property?.Href != null && !string.IsNullOrWhiteSpace(property.Text);

    public static bool ValidateProperty(this ContentReference property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit || property != null;

    public static bool ValidateProperty(this Url property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit || property != null;

    public static bool ValidateProperty(this LinkItemCollection property) => property != null && property.Any();

    public static bool ValidateProperty<T>(this IEnumerable<T> property) => property != null && property.Any();

    public static bool ValidateProperty(this DateTime property) =>
        _contextModeResolver.Service.CurrentMode == ContextMode.Edit || property != DateTime.MinValue;

    public static bool ValidateProperty<T>(this List<T> property) => property != null && property.Any();

    public static string LimitString(this string input, int limit)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        if (input.Length <= limit)
        {
            return input;
        }

        input = input.Substring(0, limit);

        var lastSpace = input.LastIndexOf(" ");
        return string.Concat(input.AsSpan(0, lastSpace), "...");
    }
}