namespace ChildFund.Web.Infrastructure.Security
{
    public static class SecurityConstants
    {
        public static string AzureCookieScheme = "azure-cookie"; // shared app cookie
        public static string AzureChallengeScheme = "azure";    // Entra ID
        public static string GoogleChallengeScheme = "google";
        public static string FacebookChallengeScheme = "facebook";
        public static string AuthProvider = "auth-provider";
    }
}