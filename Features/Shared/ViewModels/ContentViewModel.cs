using ChildFund.Core.Settings;
using ChildFund.Features.Home;
using ChildFund.Features.Shared.Interfaces;
using ChildFund.Features.Shared.Pages;
using ChildFund.Infrastructure;
using ChildFund.Infrastructure.Cms.Helpers;
using ChildFund.Infrastructure.Cms.Services;
using ChildFund.Infrastructure.SchemaMarkup;
using EPiServer.Globalization;

namespace ChildFund.Features.Shared.ViewModels;

public class ContentViewModel<TContent> : IContentViewModel<TContent> where TContent : IContent
{
    private Injected<IContentLoader> _contentLoader;
    private Injected<IContentVersionRepository> _contentVersion;
    private Injected<IContextModeResolver> _contextModeResolver;
    private Injected<IDataService> _dataService;
    //private Injected<IGraphService> _graphService;
    private HomePage _startPage;

    public ContentViewModel() : this(default)
    {
    }

    public ContentViewModel(TContent currentContent)
    {
        CurrentContent = currentContent;
        Layout = SettingsHelper.LayoutSettings;
        Labels = SettingsHelper.LabelSettings;
        SiteSettings = SettingsHelper.SiteSettings;
        PageReferences = SettingsHelper.ReferencePageSettings;

        if (currentContent is FoundationPageData)
        {
            var foundationPage = currentContent as FoundationPageData;
            HideHeader = foundationPage.HideSiteHeader;
            HideFooter = foundationPage.HideSiteFooter;
            TrackOdp = foundationPage.TrackOdp;
            if (foundationPage.ShowBreadcrumbs)
            {
                
            }
        }
        //ELSE IF COMMERCE PAGE -- IF NEEDED
    }

    public TContent CurrentContent { get; set; }

    public bool HideHeader { get; set; }

    public bool HideFooter { get; set; }

    public bool TrackOdp { get; set; }

    public virtual HomePage StartPage
    {
        get
        {
            if (_startPage == null)
            {
                ContentReference currentStartPageLink = ContentReference.StartPage;
                if (CurrentContent != null)
                {
                    currentStartPageLink = CurrentContent.GetRelativeStartPage();
                }

                if (_contextModeResolver.Service.CurrentMode.EditOrPreview())
                {
                    var startPageRef = _contentVersion.Service.LoadCommonDraft(currentStartPageLink,
                        ContentLanguage.PreferredCulture.Name);
                    if (startPageRef == null)
                    {
                        _startPage = _contentLoader.Service.Get<HomePage>(currentStartPageLink);
                    }
                    else
                    {
                        _startPage = _contentLoader.Service.Get<HomePage>(startPageRef.ContentLink);
                    }
                }
                else
                {
                    _startPage = _contentLoader.Service.Get<HomePage>(currentStartPageLink);
                }
            }

            return _startPage;
        }
    }

    public LayoutSettings Layout { get; set; }

    public LabelSettings Labels { get; set; }

    public SiteSettings SiteSettings { get; set; }

    public ReferencePageSettings PageReferences { get; set; }

    public IHtmlContent SchemaMarkup
    {
        get
        {
            //See if there's a schema data mapper for this content type and, if so, generate some schema markup
            if (ServiceLocator.Current.TryGetExistingInstance(out ISchemaDataMapper<TContent> mapper))
            {
                string breadcrumbsList = String.Empty;
                string faq = String.Empty;
                string itemList = String.Empty;
                
                //Replace ampersand encode for search page. It won't work encoded.
                var currentMapper = mapper.Map(CurrentContent).ToHtmlEscapedString().Replace(@"\u0026searchTerm", "&searchTerm");
                return new HtmlString(
                    $"<script type=\"application/ld+json\">{currentMapper}</script>{breadcrumbsList}{faq}{itemList}");
            }

            return new HtmlString(string.Empty);
        }
    }
}

public static class ContentViewModel
{
    public static ContentViewModel<T> Create<T>(T content) where T : IContent => new(content);
}