namespace ChildFund.Web.Core.CustomRoutes.Error;

public class NotFoundHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public NotFoundHandlerMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, NotFoundHandler requestHandler)
    {
        await _next(context);

        if (context.Response.StatusCode >= 400)
        {
            requestHandler.Handle(context);
            await _next(context);
        }
    }
}