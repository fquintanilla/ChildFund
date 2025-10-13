using EPiServer.Forms.Implementation.Elements;

namespace ChildFund.Core.Forms.CustomElements;

[ContentType(
    DisplayName = "Site URL",
    GroupName = "Custom Elements",
    GUID = "5E17E26B-0601-4BEF-B06E-12082218CFB9",
    AvailableInEditMode = false)]
public class SiteUrlPredefinedHiddenElementBlock : PredefinedHiddenElementBlock
{
    public override string PredefinedValue { get; set; }

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);
        PredefinedValue = SiteDefinition.Current?.SiteUrl?.OriginalString;
    }
}