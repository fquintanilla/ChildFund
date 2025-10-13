namespace ChildFund.Web.Infrastructure.Display;

public static class DisplayExtensions
{
    public static void AddDisplay(this IServiceCollection services)
    {
        services.Configure<DisplayOptions>(displayOption =>
        {
            displayOption.Add("full", "/displayoptions/full", "col-12", "", "epi-icon__layout--full");
        });

        services.AddDisplayResolutions();
    }

    private static void AddDisplayResolutions(this IServiceCollection services)
    {
        services.AddSingleton<StandardResolution>();
        services.AddSingleton<IpadHorizontalResolution>();
        services.AddSingleton<IphoneVerticalResolution>();
        services.AddSingleton<Iphone11Resolution>();
        services.AddSingleton<IphoneVerticalResolution>();
        services.AddSingleton<IpadAirResolution>();
        services.AddSingleton<AndroidVerticalResolution>();
    }
}