using ChildFund.Core.Settings;
using ChildFund.Infrastructure.Cms.Settings;

namespace ChildFund.Infrastructure.Cms.Helpers;

public static class SettingsHelper
{
    private static readonly Lazy<ISettingsService> _settingsService =
        new(() => ServiceLocator.Current.GetInstance<ISettingsService>());

    public static LabelSettings LabelSettings => _settingsService.Value.GetSiteSettings<LabelSettings>();

    public static LayoutSettings LayoutSettings => _settingsService.Value.GetSiteSettings<LayoutSettings>();

    public static ReferencePageSettings ReferencePageSettings =>
        _settingsService.Value.GetSiteSettings<ReferencePageSettings>();

    public static SiteSettings SiteSettings => _settingsService.Value.GetSiteSettings<SiteSettings>();

    public static SearchSettings SearchSettings => _settingsService.Value.GetSiteSettings<SearchSettings>();

    public static IConfiguration Configuration => ServiceLocator.Current.GetInstance<IConfiguration>();
}