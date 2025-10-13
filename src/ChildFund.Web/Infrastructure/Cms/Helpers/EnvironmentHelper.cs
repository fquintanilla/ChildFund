namespace ChildFund.Web.Infrastructure.Cms.Helpers;

public static class EnvironmentHelper
{
    public static bool IsLocal()
    {
        var environmentName = GetEnvironmentName();
        return string.IsNullOrEmpty(environmentName) || environmentName.Equals("Development");
    }

    public static bool IsIntegration()
    {
        var environmentName = GetEnvironmentName();
        return !string.IsNullOrEmpty(environmentName) && environmentName.Equals("Integration");
    }

    public static bool IsPreproduction()
    {
        var environmentName = GetEnvironmentName();
        return !string.IsNullOrEmpty(environmentName) && environmentName.Equals("Preproduction");
    }

    public static bool IsProduction()
    {
        var environmentName = GetEnvironmentName();
        return !string.IsNullOrEmpty(environmentName) && environmentName.Equals("Production");
    }

    public static string GetEnvironmentName() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    public static string GetEnvironmentPrefixForGraph() => IsProduction() ? "prd" : IsPreproduction() ? "stg" : "inte";

    public static string GetODPApiKey()
    {
        var config = ServiceLocator.Current.GetInstance<IConfiguration>();
        return config["Odp:ApiKey"];
    }
}