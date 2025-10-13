using ChildFund.Core.Settings;
using ChildFund.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class SettingsServiceExtensions
{
    private static readonly Lazy<ISettingsService> _settingsService =
        new(() => ServiceLocator.Current.GetInstance<ISettingsService>());

    public static LayoutSettings GetLayoutSettings(this IHtmlHelper helper) =>
        _settingsService.Value.GetSiteSettings<LayoutSettings>();

    public static T GetSiteSettingsOrThrow<T>(this ISettingsService settingsService,
        Func<T, bool> shouldThrow,
        string message) where T : SettingsBase
    {
        var settings = settingsService.GetSiteSettings<T>();
        if (settings == null || (shouldThrow?.Invoke(settings) ?? false))
        {
            throw new InvalidOperationException(message);
        }

        return settings;
    }

    public static bool TryGetSiteSettings<T>(this ISettingsService settingsService, out T value) where T : SettingsBase
    {
        value = settingsService.GetSiteSettings<T>();
        return value != null;
    }
}