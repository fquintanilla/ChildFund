namespace ChildFund.Infrastructure.Cms.Helpers;

public static class LoginStatusHelper
{
    /// <summary>
    /// Validate if a website user is logged in
    /// TODO: Add methods to check if basic auth, if SSO, if admin so we can use in different controllers
    /// </summary>
    public static bool IsUserLoggedIn(IPrincipal user) => user.Identity != null && user.Identity.IsAuthenticated;
}