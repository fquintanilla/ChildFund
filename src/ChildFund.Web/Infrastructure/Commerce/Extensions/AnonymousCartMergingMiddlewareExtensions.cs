﻿namespace ChildFund.Web.Infrastructure.Commerce.Extensions
{
    public static class AnonymousCartMergingMiddlewareExtensions
    {
        public static IApplicationBuilder UseAnonymousCartMerging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AnonymousCartMergingMiddleware>();
        }
    }
}
