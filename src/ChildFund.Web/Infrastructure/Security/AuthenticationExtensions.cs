using System.Security.Claims;
using ChildFund.Web.Infrastructure.Cms.Users;
using EPiServer.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace ChildFund.Web.Infrastructure.Security
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Use the built-in Optimizely CMS Identity (local users stored in EPiServerDB).
        /// </summary>
        public static IServiceCollection UseOptimizelyCmsIdentity<TUser>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TUser : SiteUser, new()
        {
            services.AddCmsAspNetIdentity<TUser>(o =>
            {
                var conn = configuration.GetConnectionString("EPiServerDB");
                if (!string.IsNullOrWhiteSpace(conn))
                {
                    o.ConnectionStringOptions = new ConnectionStringOptions
                    {
                        Name = "EPiServerDB",
                        ConnectionString = conn
                    };
                }
            });

            return services;
        }

        /// <summary>
        /// Configure Entra ID (formerly Azure AD) for CMS 12 admin/auth.
        /// Reads settings from configuration keys:
        /// Authentication:AzureClientID
        /// Authentication:AzureClientSecret
        /// Authentication:azureAuthority       (e.g. https://login.microsoftonline.com/{tenantId}/v2.0)
        /// Authentication:CallbackPath         (e.g. /signin-oidc)
        /// </summary>
        public static IServiceCollection UseEntraIdForCms(this IServiceCollection services, IConfiguration configuration)
        {
            // Cookie and scheme names used below
            const string cookieScheme = "azure-cookie";
            const string challengeScheme = "azure";

            // Keep CMS login/logout paths
            services.ConfigureApplicationCookie(c =>
            {
                c.LoginPath = "/Login";
                c.LogoutPath = "/Logout";
            });

            // (Optional, helpful while wiring up) — shows details in logs
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            // Pull credentials from configuration
            var clientId = configuration["Authentication:AzureClientID"];
            var clientSecret = configuration["Authentication:AzureClientSecret"];
            var azureAuthority = configuration["Authentication:AzureAuthority"];
            var callbackPath = configuration["Authentication:CallbackPath"] ?? "/signin-oidc";

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = cookieScheme;
                options.DefaultChallengeScheme = challengeScheme;
            })
            .AddCookie(cookieScheme, options =>
            {
                // Sync user and roles as soon as the cookie is issued
                options.Events = new CookieAuthenticationEvents
                {
                    OnSignedIn = async ctx =>
                    {
                        if (ctx.Principal?.Identity is ClaimsIdentity claimsIdentity)
                        {
                            var sync = ctx.HttpContext.RequestServices.GetRequiredService<ISynchronizingUserService>();
                            await sync.SynchronizeAsync(claimsIdentity);
                        }
                    }
                };
            })
            .AddOpenIdConnect(challengeScheme, options =>
            {
                options.SignInScheme = cookieScheme;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.UsePkce = true;

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.Authority = azureAuthority;
                options.CallbackPath = new PathString(callbackPath);

                // Scopes and claims mapping
                options.Scope.Clear();
                options.Scope.Add(OpenIdConnectScope.OpenIdProfile);
                options.Scope.Add(OpenIdConnectScope.OfflineAccess);
                options.Scope.Add(OpenIdConnectScope.Email);

                options.MapInboundClaims = false; // we’ll control claim types explicitly
                options.GetClaimsFromUserInfoEndpoint = true; // pull extra claims after code flow

                // Map OpenID claims to .NET claim types used by Optimizely’s synchronizer
                options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");

                // Optional fallbacks (some tenants don’t emit 'email')
                options.ClaimActions.MapCustomJson(ClaimTypes.Email, user =>
                {
                    return user.TryGetProperty("email", out var e) ? e.GetString()
                        : user.TryGetProperty("preferred_username", out var u) ? u.GetString()
                        : null;
                });

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RoleClaimType = "roles",
                    NameClaimType = "preferred_username",
                    ValidateIssuer = false
                };

                // Event wiring (matches the behavior in your diff)
                options.Events = new OpenIdConnectEvents
                {
                    // Prevent redirect loop when the response is already being handled
                    OnRedirectToIdentityProvider = ctx =>
                    {
                        if (ctx.Response.StatusCode == StatusCodes.Status401Unauthorized)
                        {
                            ctx.HandleResponse();
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.BodyWriter.WriteAsync(
                            Encoding.ASCII.GetBytes(context.Exception.Message));
                        return Task.CompletedTask;
                    },
                    // OnTokenValidated (Entra) already syncs users/roles. For Google, the handler doesn't
                    // raise that same event; however, the cookie gets issued via the same azure-cookie.
                    // We already sync on cookie sign-in in your AddCookie(...).Events.OnSignedIn handler.
                    OnTokenValidated = ctx =>
                    {
                        var redirect = ctx.Properties?.RedirectUri;
                        if (!string.IsNullOrEmpty(redirect) &&
                            Uri.TryCreate(redirect, UriKind.RelativeOrAbsolute, out var uri) &&
                            uri.IsAbsoluteUri)
                        {
                            ctx.Properties.RedirectUri = uri.PathAndQuery;
                        }

                        // Tag principal with provider
                        if (ctx.Principal?.Identity is ClaimsIdentity id)
                        {
                            id.AddClaim(new Claim(SecurityConstants.AuthProvider, "entra"));
                        }

                        // Background sync of user + roles to Optimizely
                        ServiceLocator.Current
                            .GetInstance<ISynchronizingUserService>()
                            .SynchronizeAsync(ctx.Principal?.Identity as ClaimsIdentity);

                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        /// <summary>
        /// Adds Google OAuth for CMS sign-in (shares the same app cookie).
        /// </summary>
        public static IServiceCollection UseGoogleForCms(this IServiceCollection services, IConfiguration configuration)
        {
            // We will sign in to the SAME cookie used for Entra ID so the rest of the app
            // only has to look for one external-auth cookie.
            services.AddAuthentication()
                .AddGoogle("google", options =>
                {
                    options.SignInScheme = SecurityConstants.AzureCookieScheme;    // write the same cookie after Google login

                    // Google credentials
                    options.ClientId = configuration["Authentication:GoogleClientID"]!;
                    options.ClientSecret = configuration["Authentication:GoogleClientSecret"]!;

                    // Ensure standard claims are present for Optimizely sync
                    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");

                    // Tag principal with provider
                    options.Events.OnCreatingTicket = ctx =>
                    {
                        if (ctx.Principal?.Identity is ClaimsIdentity id)
                        {
                            id.AddClaim(new Claim(SecurityConstants.AuthProvider, "google"));
                        }
                        return Task.CompletedTask;
                    };
                });

            return services;
        }
    }
}