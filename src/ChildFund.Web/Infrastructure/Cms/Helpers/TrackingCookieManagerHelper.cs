namespace ChildFund.Web.Infrastructure.Cms.Helpers;

public static class TrackingCookieManagerHelper
{
    public static string TrackingCookieName = "_madid";

    public static string GetTrackingCookie()
    {
        var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
        if (accessor.HttpContext == null)
        {
            return string.Empty;
        }

        var cookie = accessor.HttpContext.Request.Cookies[TrackingCookieName];
        return cookie ?? string.Empty;
    }

    public static void SetTrackingCookie(string value)
    {
        var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
        accessor.HttpContext?.Response.Cookies.Append(TrackingCookieName, value);
    }
}