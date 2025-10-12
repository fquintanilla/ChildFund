namespace ChildFund.Core.CustomRoutes.Error;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context, NotFoundHandler notFoundHandler, ErrorHandler errorHandler)
    {
        switch (context.Response.StatusCode)
        {
            case >= 500:
                errorHandler.Handle(context);
                await _next(context);
                break;
            case >= 400:
                notFoundHandler.Handle(context);
                await _next(context);
                break;
            default:
                await _next(context);
                break;
        }
    }
}