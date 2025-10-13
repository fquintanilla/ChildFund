namespace ChildFund.Web.Infrastructure.Middlewares;

public class RestrictImageResizeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _maxWidth;
    private readonly int _maxHeight;

    public RestrictImageResizeMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _maxWidth = configuration?.GetValue<int>("ImageSizeOptions:MaxWidth") ?? 1000;
        _maxHeight = configuration?.GetValue<int>("ImageSizeOptions:MaxHeight") ?? 1000;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var width = context.Request.Query["width"];
        var height = context.Request.Query["height"];

        if (!string.IsNullOrEmpty(width) || !string.IsNullOrEmpty(height))
        {
            var isWNumeric = decimal.TryParse(width, out var widthValue);
            var isHNumeric = decimal.TryParse(height, out var heightValue);

            if (!string.IsNullOrEmpty(width) && !isWNumeric ||
                !string.IsNullOrEmpty(height) && !isHNumeric)
            {
                context.Response.Redirect($"{context.Request.Path}");
                return;
            }

            if (widthValue > _maxWidth || heightValue > _maxHeight)
            {
                var queryString = context.Request.QueryString.Value;
                queryString = queryString?.Replace($"width={width}", "").Replace($"height={height}", "").Replace("&&", "&").Trim('&', '?');

                if (!string.IsNullOrWhiteSpace(queryString))
                {
                    var newUri = $"{context.Request.Path}?{queryString}";
                    context.Response.Redirect(newUri);
                    return;
                }
                else
                {
                    var newUri = $"{context.Request.Path}";
                    context.Response.Redirect(newUri);
                    return;
                }
            }
        }

        await _next(context);
    }
}
