using ChildFund.Infrastructure.Cms.Helpers;

namespace ChildFund.Core.CustomRoutes.Error;

public class ErrorHandler
{
    #region Properties

    private readonly IUrlResolver _urlResolver;

    #endregion

    #region Constructor

    public ErrorHandler(IUrlResolver urlResolver) => _urlResolver = urlResolver;

    #endregion

    public virtual bool Handle(HttpContext context)
    {
        if (context == null)
        {
            return false;
        }

        var reference = SettingsHelper.ReferencePageSettings;
        if (reference != null && !ContentReference.IsNullOrEmpty(reference.Error500))
        {
            context.Request.Path = _urlResolver.GetUrl(reference.Error500);
            return true;
        }

        return false;
    }
}