using ChildFund.Web.Infrastructure.Security;
using EPiServer.Shell.Security;
using Microsoft.AspNetCore.Authentication;

namespace ChildFund.Web.Features.Login
{
    public class LoginController(
        UrlResolver urlResolver,
        UISignInManager signInManager) : PageController<LoginPage>
    {
        public IActionResult Index([FromQuery] string returnUrl = "/")
            => Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "azure");

        public IActionResult Google([FromQuery] string returnUrl = "/") =>
            Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "google");

        public IActionResult Facebook([FromQuery] string returnUrl = "/") =>
            Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "facebook");

        public async Task<IActionResult> Logout()
        {
            var authProvider = User.FindFirst(SecurityConstants.AuthProvider)?.Value;

            if (string.IsNullOrEmpty(authProvider))
            {
                await signInManager.SignOutAsync();
            }
            else // Entra ID or Google
            {
                await ControllerContext.HttpContext.SignOutAsync(SecurityConstants.AzureCookieScheme);
                HttpContext.Response.Cookies.Delete($".AspNetCore.{SecurityConstants.AzureCookieScheme}");
            }

            return Redirect(urlResolver.GetUrl(PageContext.ContentLink, PageContext.LanguageID) ?? "/");
        }
    }
}