using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ChildFund.Web.Infrastructure.Security
{
    public static class DualAuthExtensions
    {
        private static readonly string IdentityCookieScheme = IdentityConstants.ApplicationScheme;

        /// <summary>
        /// After BOTH UseOptimizelyCmsIdentity and UseEntraIdForCms are registered,
        /// call this to: 
        ///  - authenticate with whichever cookie is present,
        ///  - challenge based on the request path (/login => Entra, /util/login => Optimizely).
        /// </summary>
        public static IServiceCollection UseDualAuthGateway(this IServiceCollection services)
        {
            services.AddAuthentication() // adds to the existing auth builder
                                         // Default AUTHENTICATE: choose the cookie we actually have
                .AddPolicyScheme("smart-auth", "Smart Auth", options =>
                {
                    options.ForwardDefaultSelector = ctx =>
                    {
                        // NOTE: cookie names are ".AspNetCore." + scheme by default
                        var cookies = ctx.Request.Cookies;
                        if (cookies.ContainsKey(".AspNetCore." + SecurityConstants.AzureCookieScheme))
                            return SecurityConstants.AzureCookieScheme;

                        if (cookies.ContainsKey(".AspNetCore." + IdentityCookieScheme))
                            return IdentityCookieScheme;

                        // no cookie yet — fall back to Azure cookie for auth checks
                        return SecurityConstants.AzureCookieScheme;
                    };
                })
                // Default CHALLENGE: choose provider by path
                .AddPolicyScheme("smart-challenge", "Smart Challenge", options =>
                {
                    options.ForwardDefaultSelector = ctx =>
                    {
                        var path = ctx.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;

                        // Your rule:
                        if (path.StartsWith("/login/google"))
                            return SecurityConstants.GoogleChallengeScheme;           // Google

                        if (path.StartsWith("/login"))
                            return SecurityConstants.AzureChallengeScheme;          // Entra ID

                        if (path.StartsWith("/util/login"))
                            return IdentityCookieScheme;          // Optimizely local login

                        // default challenge elsewhere -> Entra
                        return SecurityConstants.AzureChallengeScheme;
                    };
                });

            // Make our policy schemes the defaults (overrides what Identity set)
            services.PostConfigure<AuthenticationOptions>(o =>
            {
                o.DefaultScheme = "smart-auth";             // how to AUTHENTICATE requests
                o.DefaultAuthenticateScheme = "smart-auth";
                o.DefaultChallengeScheme = "smart-challenge"; // how to CHALLENGE when login is needed
                o.DefaultSignInScheme = SecurityConstants.AzureCookieScheme;  // OIDC writes this cookie after login
            });

            return services;
        }
    }
}
