using ChildFund.Web.Core.Media.Models;
using ChildFund.Web.Features.Common.Pages.Folder;
using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Infrastructure.Cms.Attributes;
using ChildFund.Web.Infrastructure.Cms.Constants;
using ChildFund.Web.Infrastructure.Cms.Settings;
using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using Newtonsoft.Json;

namespace ChildFund.Web.Core.Settings;

[SettingsContentType(DisplayName = "Scripts Injection Settings",
    GUID = "0156b963-88a9-450b-867c-e5c5e7be29fd",
    Description = "Scripts Injection Settings",
    SettingsName = "Scripts Injection")]
public class ScriptInjectionSettings : SettingsBase
{
    #region Scripts

    [JsonIgnore]
    [CultureSpecific]
    [Display(Name = "Header Scripts (Scripts will inject at the bottom of header)", GroupName = TabNames.Scripts,
        Description = "Scripts will inject at the bottom of header", Order = 10)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
    [Searchable(false)]
    public virtual IList<ScriptInjectionModel> HeaderScripts { get; set; }

    [JsonIgnore]
    [CultureSpecific]
    [Display(Name = "Body Scripts (Scripts will inject at content of the page)", GroupName = TabNames.Scripts,
        Description = "Scripts will inject at the content of the page", Order = 20)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
    [Searchable(false)]
    public virtual IList<ScriptInjectionModel> BodyScripts { get; set; }

    [JsonIgnore]
    [CultureSpecific]
    [Display(Name = "Footer Scripts (Scripts will inject at the bottom of footer)", GroupName = TabNames.Scripts,
        Description = "Scripts will inject at the bottom of footer", Order = 30)]
    [EditorDescriptor(EditorDescriptorType = typeof(CollectionEditorDescriptor<ScriptInjectionModel>))]
    [Searchable(false)]
    public virtual IList<ScriptInjectionModel> FooterScripts { get; set; }

    #endregion
}

public class ScriptInjectionModel
{
    [RequiredForPublish]
    [CultureSpecific]
    [AllowedTypes(typeof(FoundationPageData), typeof(FolderPage))]
    [Display(Name = "Root (Scripts will inject for this page and all children pages)",
        Description = "Scripts will inject for this page and all children pages", Order = 10)]
    public virtual ContentReference ScriptRoot { get; set; }

    [AllowedTypes(typeof(CodingFile))]
    [Display(Name = "Script files", Order = 20)]
    public virtual IList<ContentReference> ScriptFiles { get; set; }

    [UIHint(UIHint.Textarea)]
    [Display(Name = "External Scripts", Order = 30)]
    public virtual string ExternalScripts { get; set; }

    [UIHint(UIHint.Textarea)]
    [Display(Name = "Inline Scripts", Order = 40)]
    public virtual string InlineScripts { get; set; }
}

[PropertyDefinitionTypePlugIn]
public class ScriptInjectionProperty : PropertyList<ScriptInjectionModel>
{
}