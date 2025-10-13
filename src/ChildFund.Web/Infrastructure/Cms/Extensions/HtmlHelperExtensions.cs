using ChildFund.Web.Core.Settings;
using ChildFund.Web.Features.Home;
using ChildFund.Web.Features.Shared.Interfaces;
using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Infrastructure.Cms.Extensions;
using ChildFund.Web.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;

namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class HtmlHelperExtensions
{
    private const string _cssFormat = "<link href=\"{0}\" rel=\"stylesheet\" />";
    private const string _scriptFormat = "<script src=\"{0}\"></script>";
    private const string _metaFormat = "<meta property=\"{0}\" content=\"{1}\" />";

    private static readonly Lazy<IContentLoader> _contentLoader =
        new(() => ServiceLocator.Current.GetInstance<IContentLoader>());

    private static readonly Lazy<IUrlResolver> _urlResolver =
        new(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

    private static readonly Lazy<IPermanentLinkMapper> _permanentLinkMapper =
        new(() => ServiceLocator.Current.GetInstance<IPermanentLinkMapper>());

    private static readonly Lazy<IDatabaseMode> _databaseMode =
        new(() => ServiceLocator.Current.GetInstance<IDatabaseMode>());

    private static readonly Lazy<ISettingsService> _settingsService =
        new(() => ServiceLocator.Current.GetInstance<ISettingsService>());

    private static readonly Lazy<IContextModeResolver> _contextModeResolver =
        new(() => ServiceLocator.Current.GetInstance<IContextModeResolver>());

    public static IHtmlContent RenderReadonlyMessage(this IHtmlHelper htmlHelper)
    {
        if (_databaseMode.Value.DatabaseMode == DatabaseMode.ReadWrite)
        {
            return htmlHelper.Raw(string.Empty);
        }

        return htmlHelper.Raw(
            $"<div class=\"container-fluid px-0\"><div class=\"alert alert-info\" role=\"alert\"><p class=\"text-center\">{LocalizationService.Current.GetString("/Readonly/Message", "The site is currently undergoing maintenance.Certain features are disabled until the maintenance has completed.")}</p></div></div>");
    }

    public static bool IsReadOnlyMode(this IHtmlHelper htmlHelper) =>
        _databaseMode.Value.DatabaseMode == DatabaseMode.ReadOnly;

    public static HtmlString RenderExtendedCss(this IHtmlHelper helper, IContent content)
    {
        if (content == null || ContentReference.StartPage == PageReference.EmptyReference ||
            !(content is IFoundationContent sitePageData))
        {
            return new HtmlString("");
        }

        var outputCss = new StringBuilder(string.Empty);
        var startPage = _contentLoader.Value.Get<HomePage>(ContentReference.StartPage);

        // Extended Css file
        AppendFiles(startPage.CssFiles, outputCss, _cssFormat);
        if (!(sitePageData is HomePage))
        {
            AppendFiles(sitePageData.CssFiles, outputCss, _cssFormat);
        }

        // Inline CSS
        if (!string.IsNullOrWhiteSpace(startPage.Css) || !string.IsNullOrWhiteSpace(sitePageData.Css))
        {
            outputCss.AppendLine("<style>");
            outputCss.AppendLine(!string.IsNullOrWhiteSpace(startPage.Css) ? startPage.Css : "");
            outputCss.AppendLine(!string.IsNullOrWhiteSpace(sitePageData.Css) && sitePageData is not HomePage
                ? sitePageData.Css
                : "");
            outputCss.AppendLine("</style>");
        }

        return new HtmlString(outputCss.ToString());
    }

    public static HtmlString RenderHeaderScripts(this IHtmlHelper helper, IContent content)
    {
        var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
        return helper.RenderScripts(content, settings?.HeaderScripts);
    }

    public static HtmlString RenderBodyScripts(this IHtmlHelper helper, IContent content)
    {
        var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
        return helper.RenderScripts(content, settings?.BodyScripts);
    }

    public static HtmlString RenderFooterScripts(this IHtmlHelper helper, IContent content)
    {
        var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
        return helper.RenderScripts(content, settings?.FooterScripts);
    }

    private static HtmlString RenderScripts(this IHtmlHelper helper, IContent content,
        IList<ScriptInjectionModel> scripts)
    {
        if (content == null || ContentReference.StartPage == PageReference.EmptyReference ||
            content is not FoundationPageData)
        {
            return new HtmlString("");
        }

        var outputScript = new StringBuilder(string.Empty);

        // Injection Hierarchically Javascript
        if (scripts != null && scripts.Any())
        {
            ProcessHierarchicallyJavascript(content, scripts, outputScript);
        }

        return new HtmlString(outputScript.ToString());
    }

    private static void ProcessHierarchicallyJavascript(IContent content, IList<ScriptInjectionModel> scripts,
        StringBuilder outputScript)
    {
        foreach (var script in scripts)
        {
            var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
            if (pages.Any(x => x == content.ContentLink) || content.ContentLink == script.ScriptRoot)
            {
                // Script Files
                AppendFiles(script.ScriptFiles, outputScript, _scriptFormat);

                // External Javascript
                if (!string.IsNullOrWhiteSpace(script.ExternalScripts))
                {
                    outputScript.AppendLine(script.ExternalScripts);
                }

                // Inline Javascript
                if (!string.IsNullOrWhiteSpace(script.InlineScripts))
                {
                    outputScript.AppendLine("<script type=\"text/javascript\">");
                    outputScript.AppendLine(
                        !string.IsNullOrWhiteSpace(script.InlineScripts) ? script.InlineScripts : "");
                    outputScript.AppendLine("</script>");
                }
            }
        }
    }

    public static IHtmlContent RenderTextArea(this IHtmlHelper htmlHelper, string textAreaString)
    {
        return new HtmlString(textAreaString?.Replace("\n", "<br/>"));
    }

    public static HtmlString RenderMetaData(this IHtmlHelper helper, IContent content)
    {
        if (content is not FoundationPageData sitePageData)
        {
            return new HtmlString("");
        }

        var output = new StringBuilder(string.Empty);

        if (!string.IsNullOrWhiteSpace(sitePageData.MetaTitle))
        {
            output.AppendLine(string.Format(_metaFormat, "title", sitePageData.MetaTitle));
        }

        if (!string.IsNullOrEmpty(sitePageData.Keywords))
        {
            output.AppendLine(string.Format(_metaFormat, "keywords", sitePageData.Keywords));
        }

        if (!string.IsNullOrWhiteSpace(sitePageData.PageDescription))
        {
            output.AppendLine(string.Format(_metaFormat, "description", sitePageData.PageDescription));
        }

        if (sitePageData.DisableIndexing)
        {
            output.AppendLine("<meta name=\"robots\" content=\"NOINDEX, NOFOLLOW\">");
        }

        return new HtmlString(output.ToString());
    }

    private static void AppendFiles(LinkItemCollection files, StringBuilder outputString, string formatString)
    {
        if (files is not { Count: > 0 })
        {
            return;
        }

        foreach (var item in files.Where(item => !string.IsNullOrEmpty(item.Href)))
        {
            var map = _permanentLinkMapper.Value.Find(new UrlBuilder(item.Href));
            outputString.AppendLine(map == null
                ? string.Format(formatString, item.GetMappedHref())
                : string.Format(formatString, _urlResolver.Value.GetUrl(map.ContentReference)));
        }
    }

    private static void AppendFiles(IList<ContentReference> files, StringBuilder outputString, string formatString)
    {
        if (files is not { Count: > 0 })
        {
            return;
        }

        foreach (var item in files.Where(item => !string.IsNullOrEmpty(_urlResolver.Value.GetUrl(item))))
        {
            var url = _urlResolver.Value.GetUrl(item);
            outputString.AppendLine(string.Format(formatString, url));
        }
    }

    public static ConditionalLink BeginConditionalLink(this IHtmlHelper helper, bool shouldWriteLink,
        IHtmlContent url, string title = null, string cssClass = null)
    {
        if (shouldWriteLink)
        {
            var linkTag = new TagBuilder("a");
            linkTag.Attributes.Add("href", url.ToString());

            if (!string.IsNullOrWhiteSpace(title))
            {
                linkTag.Attributes.Add("title", helper.Encode(title));
            }

            if (!string.IsNullOrWhiteSpace(cssClass))
            {
                linkTag.Attributes.Add("class", cssClass);
            }

            helper.ViewContext.Writer.Write(linkTag.RenderSelfClosingTag());
        }

        return new ConditionalLink(helper.ViewContext, shouldWriteLink);
    }

    public static ConditionalLink BeginConditionalLink(this IHtmlHelper helper, bool shouldWriteLink,
        Func<IHtmlContent> urlGetter, string title = null, string cssClass = null)
    {
        IHtmlContent url = HtmlString.Empty;

        if (shouldWriteLink)
        {
            url = urlGetter();
        }

        return helper.BeginConditionalLink(shouldWriteLink, url, title, cssClass);
    }

    public static IHtmlContent MenuList(
        this IHtmlHelper helper,
        ContentReference rootLink,
        Func<MenuItem, HelperResult> itemTemplate = null,
        bool includeRoot = false,
        bool requireVisibleInMenu = true,
        bool requirePageTemplate = true)
    {
        itemTemplate ??= GetDefaultItemTemplate(helper);
        var currentContentLink = helper.ViewContext.HttpContext.GetContentLink();

        Func<IEnumerable<PageData>, IEnumerable<PageData>> filter =
            pages => pages.FilterForDisplay(requirePageTemplate, requireVisibleInMenu);

        var pagePath = _contentLoader.Value.GetAncestors(currentContentLink)
            .Reverse()
            .Select(x => x.ContentLink)
            .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
            .ToList();

        var menuItems = _contentLoader.Value.GetChildren<PageData>(rootLink)
            .FilterForDisplay(requirePageTemplate, requireVisibleInMenu)
            .Select(x => CreateMenuItem(x, currentContentLink, pagePath, _contentLoader.Value, filter))
            .ToList();

        if (includeRoot)
        {
            menuItems.Insert(0,
                CreateMenuItem(_contentLoader.Value.Get<PageData>(rootLink), currentContentLink, pagePath,
                    _contentLoader.Value,
                    filter));
        }

        var buffer = new StringBuilder();
        var writer = new StringWriter(buffer);
        foreach (var menuItem in menuItems)
        {
            itemTemplate(menuItem).WriteTo(writer, HtmlEncoder.Default);
        }

        return new HtmlString(buffer.ToString());
    }

    public static bool IsInEditMode(this IHtmlHelper htmlHelper) =>
        _contextModeResolver.Value.CurrentMode == ContextMode.Edit;


    private static MenuItem CreateMenuItem(PageData page, ContentReference currentContentLink,
        List<ContentReference> pagePath, IContentLoader contentLoader,
        Func<IEnumerable<PageData>, IEnumerable<PageData>> filter)
    {
        var menuItem = new MenuItem(page)
        {
            Selected = page.ContentLink.CompareToIgnoreWorkID(currentContentLink) ||
                       pagePath.Contains(page.ContentLink),
            HasChildren =
                new Lazy<bool>(() => filter(contentLoader.GetChildren<PageData>(page.ContentLink)).Any())
        };
        return menuItem;
    }

    private static Func<MenuItem, HelperResult> GetDefaultItemTemplate(IHtmlHelper helper) => x =>
        new HelperResult(async writer => await writer.WriteAsync(helper.PageLink(x.Page).ToString()));
}

public class MenuItem
{
    public MenuItem(PageData page) => Page = page;
    public PageData Page { get; set; }
    public bool Selected { get; set; }
    public Lazy<bool> HasChildren { get; set; }
}

public class ConditionalLink : IDisposable
{
    private readonly bool _linked;
    private readonly ViewContext _viewContext;
    private bool _disposed;

    public ConditionalLink(ViewContext viewContext, bool isLinked)
    {
        _viewContext = viewContext;
        _linked = isLinked;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (_linked)
        {
            _viewContext.Writer.Write("</a>");
        }
    }
}