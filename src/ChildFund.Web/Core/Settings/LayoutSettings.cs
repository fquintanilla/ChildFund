using ChildFund.Web.Infrastructure.Cms.Constants;
using ChildFund.Web.Infrastructure.Cms.Settings;
using Geta.Optimizely.ContentTypeIcons;
using Geta.Optimizely.ContentTypeIcons.Attributes;

namespace ChildFund.Web.Core.Settings;

[SettingsContentType(DisplayName = "Layout Settings",
    GUID = "f7366060-c801-494c-99b8-b761ac3447c3",
    Description = "Header settings, footer settings, menu settings",
    AvailableInEditMode = true,
    SettingsName = "Layout Settings")]
[ContentTypeIcon(FontAwesome5Solid.WindowMaximize)]
public class LayoutSettings : SettingsBase
{
    #region Cache
    [Display(Name = "CSS cache version",
        Description = "Set this value to a different string to force expire CSS cache without a CDN cache clear",
        GroupName = TabNames.Cache, Order = 95)]
    [Searchable(false)]
    public virtual string CssCacheVersion { get; set; }

    #endregion

    #region Header

    #endregion

    #region Footer

    #endregion
}