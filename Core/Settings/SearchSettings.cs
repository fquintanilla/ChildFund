using ChildFund.Infrastructure.Cms.Settings;
using Geta.Optimizely.ContentTypeIcons;
using Geta.Optimizely.ContentTypeIcons.Attributes;

namespace ChildFund.Core.Settings;

[SettingsContentType(DisplayName = "Search Settings",
    GUID = "d4171337-70a4-476a-aa3c-0d976ac185e8",
    SettingsName = "Search Settings")]
[ContentTypeIcon(FontAwesome5Solid.Search)]
public class SearchSettings : SettingsBase
{

}