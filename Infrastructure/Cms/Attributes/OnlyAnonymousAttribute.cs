using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ChildFund.Infrastructure.Cms.Attributes;

public class OnlyAnonymousAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
        if (context.HttpContext.User.Identity is { IsAuthenticated: true })
        {
            context.Result = new ForbidResult();
        }
    }
}