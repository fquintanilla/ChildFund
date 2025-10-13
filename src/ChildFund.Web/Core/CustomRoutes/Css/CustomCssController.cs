using ChildFund.Web.Infrastructure.Cms.Helpers;

namespace ChildFund.Web.Core.CustomRoutes.Css;

public class CustomCssController : Controller
{
    [Route("/css/custom.css")]
    public async Task<IActionResult> CustomCss()
    {
        var settings = await Task.Run(() => SettingsHelper.SiteSettings);

        if (settings == null)
        {
            return null;
        }

        var encoding = "text/css; charset=utf-8";

        if (string.IsNullOrEmpty(settings.CustomCss))
        {
            return File(Array.Empty<byte>(), encoding);
        }

        return File(Encoding.UTF8.GetBytes(settings.CustomCss), encoding);
    }
}