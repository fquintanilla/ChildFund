using ChildFund.Infrastructure.Cms.Attributes;
using ChildFund.Infrastructure.Cms.Settings;
using Geta.Optimizely.ContentTypeIcons;
using Geta.Optimizely.ContentTypeIcons.Attributes;

namespace ChildFund.Core.Settings;

[SettingsContentType(DisplayName = "Site Settings",
    GUID = "d41713b7-70a4-476a-ab3c-0d976ac189e8",
    SettingsName = "Site Settings")]
[ContentTypeIcon(FontAwesome5Solid.Cog)]
public class SiteSettings : SettingsBase
{
    public static string CompanyNameDefault = "ChildFund";

    [CultureSpecific]
    [RequiredForPublish]
    [Display(Name = "Company Name", Order = 50)]
    [Searchable(false)]
    public virtual string CompanyName
    {
        get
        {
            var value = this.GetPropertyValue(p => p.CompanyName);

            return !string.IsNullOrWhiteSpace(value)
                ? value
                : CompanyNameDefault;
        }
        set => this.SetPropertyValue(p => p.CompanyName, value);
    }

    [UIHint(UIHint.Textarea)]
    [Display(Name = "Custom CSS",
        Description = "Editor managed CSS. WARNING: You may cause issues if not set properly.",
        GroupName = SystemTabNames.Content, Order = 100)]
    [Searchable(false)]
    public virtual string CustomCss { get; set; }

    [Display(Name = "Enable ODP Tracking", GroupName = SystemTabNames.Content, Order = 600)]
    [Searchable]
    public virtual bool EnableOdpTracking { get; set; }

	#region Tags - Facets

	#endregion


	#region Modals

	#endregion

	#region Commerce

	#endregion

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);
        CompanyName = CompanyNameDefault;
    }
}