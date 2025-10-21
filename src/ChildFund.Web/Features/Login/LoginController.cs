using ChildFund.Web.Features.Shared.ViewModels;
using Microsoft.AspNetCore.Authentication;

namespace ChildFund.Web.Features.Login
{
    public class LoginController(UrlResolver urlResolver) : PageController<LoginPage>
    {
        public IActionResult Index([FromQuery] string returnUrl = "/")
            => Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "azure");

        public async Task<IActionResult> Logout()
        {
            //comment out old logout code
            //await UISignInManager.Service.SignOutAsync();
            //return Redirect(HttpContext.RequestServices.GetService<UrlResolver>().GetUrl(PageContext.ContentLink, PageContext.LanguageID));
            await ControllerContext.HttpContext.SignOutAsync("azure-cookie");
            HttpContext.Response.Cookies.Delete($".AspNetCore.{"azure-cookie"}");
            return Redirect(urlResolver.GetUrl(PageContext.ContentLink, PageContext.LanguageID) ?? "/");
        }
    }
}