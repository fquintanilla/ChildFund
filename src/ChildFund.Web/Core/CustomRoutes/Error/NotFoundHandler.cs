using ChildFund.Web.Infrastructure.Cms.Helpers;

namespace ChildFund.Web.Core.CustomRoutes.Error;

public class NotFoundHandler
{
    #region Properties

    private readonly IUrlResolver _urlResolver;

    #endregion

    #region Constructor

    public NotFoundHandler(IUrlResolver urlResolver) => _urlResolver = urlResolver;

    #endregion

    public virtual void Handle(HttpContext context)
    {
        if (context?.Response == null || context.Response.StatusCode != 404)
        {
            return;
        }

        var reference = SettingsHelper.ReferencePageSettings;
        if (reference != null && !ContentReference.IsNullOrEmpty(reference.Error404))
        {
            context.Request.Path = _urlResolver.GetUrl(reference.Error404);
        }
    }
}