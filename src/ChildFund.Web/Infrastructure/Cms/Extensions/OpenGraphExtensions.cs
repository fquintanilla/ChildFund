using Boxed.AspNetCore.TagHelpers.OpenGraph;
using ChildFund.Web.Core.Settings;
using ChildFund.Web.Features.Shared.Interfaces;
using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Infrastructure.Cms.Helpers;
using ChildFund.Web.Infrastructure.Cms.Settings;
using EPiServer.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class OpenGraphExtensions
{
    private static readonly Lazy<IContentLoader> _contentLoader =
        new(() => ServiceLocator.Current.GetInstance<IContentLoader>());

    private static readonly Lazy<ISettingsService> _settingsService =
        new(() => ServiceLocator.Current.GetInstance<ISettingsService>());

    private static readonly Lazy<LanguageResolver> _cultureAccessor =
        new(() => ServiceLocator.Current.GetInstance<LanguageResolver>());

    public static IHtmlContent RenderOpenGraphMetaData(this IHtmlHelper helper,
        IContentViewModel<IContent> contentViewModel)
    {
        var foundationPageContent = contentViewModel.CurrentContent as FoundationPageData;
        var metaTitle = string.IsNullOrEmpty(foundationPageContent?.MetaTitle) ? contentViewModel.CurrentContent.Name : foundationPageContent.MetaTitle;
        var openGraphTitle = string.IsNullOrEmpty(foundationPageContent?.OpenGraphTitle) ? metaTitle : foundationPageContent.OpenGraphTitle;
        var defaultLocale = _cultureAccessor.Value.GetPreferredCulture();
        IEnumerable<string> alternateLocales = null;
        string contentType = null;
        string imageUrl;

        if (contentViewModel.CurrentContent is FoundationPageData currentContent && currentContent.OpenGraphImage != null)
        {
            imageUrl = GetUrl(currentContent.OpenGraphImage);
        }
        else
        {
            imageUrl = GetDefaultImageUrl();
        }

        if (contentViewModel.CurrentContent is FoundationPageData pageData)
        {
            alternateLocales = pageData.ExistingLanguages.Where(culture => culture.Name != defaultLocale.Name)
                .Select(culture => culture.TextInfo.CultureName.Replace('-', '_'));
        }

        switch (contentViewModel.CurrentContent) //Add types if needed for customization
        {
            case FoundationPageData foundationPageData:
                var openGraphFoundationPage =
                    new FoundationPageDataOpenGraph(openGraphTitle, new OpenGraphImage(new Uri(imageUrl)),
                        GetUrl(foundationPageData.ContentLink))
                    {
                        Description = foundationPageData.PageDescription,
                        Locale = defaultLocale.Name.Replace('-', '_'),
                        AlternateLocales = alternateLocales,
                        Author = foundationPageData.AuthorMetaData,
                        ModifiedTime = foundationPageData.Changed,
                        PublishedTime = foundationPageData.StartPublish,
                        ExpirationTime = foundationPageData.StopPublish
                    };

                return helper.OpenGraph(openGraphFoundationPage);
        }

        return new HtmlString(string.Empty);
    }

    private static string GetDefaultImageUrl()
    {
        var layoutSettings = _settingsService.Value.GetSiteSettings<LayoutSettings>();
        return "https://via.placeholder.com/150";
        //TODO: Add site logo here
        /*var header = layoutSettings?.HeaderElement?.FilteredItems?.GetContentItems<HeaderBlock>()?.FirstOrDefault();
        if (header?.SiteLogo.IsNullOrEmpty() ?? true)
        {
            return "https://via.placeholder.com/150";
        }

        var siteUrl = SiteDefinitionHelper.GetPrimaryHostUri();
        var url = new Uri(siteUrl, UrlResolver.Current.GetUrl(header.SiteLogo));

        return url.ToString();*/
        return string.Empty;
    }

    private static string GetUrl(ContentReference content)
    {
        var siteUrl = SiteDefinitionHelper.GetPrimaryHostUri();
        var url = new Uri(siteUrl, UrlResolver.Current.GetUrl(content));

        return url.ToString();
    }

    private static IEnumerable<string> GetCategoryNames(IEnumerable<ContentReference> categories)
    {
        if (categories == null)
        {
            yield break;
        }

        foreach (var category in categories)
        {
            yield return _contentLoader.Value.Get<IContent>(category).Name;
        }
    }
}