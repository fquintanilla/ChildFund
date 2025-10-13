using ChildFund.Web.Infrastructure.Cms.Extensions;

namespace ChildFund.Web.Infrastructure.Cms.Helpers;

public static class LanguageHelper
{
    private static readonly Lazy<ILanguageBranchRepository> _languageBranchRepository =
        new(() => ServiceLocator.Current.GetInstance<ILanguageBranchRepository>());

    /// <summary>
    ///     Returns the list of site languages
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Language> GetSiteLanguages()
    {
        var site = SiteDefinition.Current;
        var startPage = site.StartPage?.Get<PageData>();
        var activeLanguages = ServiceLocator.Current.GetInstance<LanguageBranchRepository>().ListEnabled();
        var siteLanguages = ((PageData)startPage)?.ExistingLanguages?.ToList();

        var lstLanguages = new List<Language>();
        foreach (var activeLanguage in activeLanguages)
        {
            var culture = new CultureInfo(activeLanguage.LanguageID);
            var currentCulture = siteLanguages?.FirstOrDefault(x => x.Name == culture.Name);
            if (currentCulture != null)
            {
                lstLanguages.Add(new Language
                {
                    Label = char.ToUpper(currentCulture.NativeName[0]) + currentCulture.NativeName.Substring(1),
                    Value = currentCulture.Name
                });
            }
        }

        return lstLanguages;
    }

    public static List<LanguageBranch> GetAvailableLanguages() =>
        _languageBranchRepository.Value.ListEnabled()?.ToList() ?? new List<LanguageBranch>();

    public static LanguageBranch GetCurrentLanguageBySite(SiteDefinition site)
    {
        var startPage = site.StartPage?.Get<PageData>();
        var availableLanguages = GetAvailableLanguages();
        var defaultLanguage = availableLanguages.FirstOrDefault();

        // If language is not detected properly by the current thread property use the content language preferred culture instead
        var currentLanguage = availableLanguages.Find(x => x.LanguageID == startPage?.LanguageBranch());
        return currentLanguage ?? defaultLanguage;
    }

    public static string GetCurrentLanguageDisplayName(string currentLanguage)
    {
        var activeLanguages = ServiceLocator.Current.GetInstance<LanguageBranchRepository>().ListEnabled();
        var found = activeLanguages.Select(x => new CultureInfo(x.LanguageID))
            .FirstOrDefault(x => x.Name == currentLanguage)?.NativeName;

        return !string.IsNullOrEmpty(found) ? char.ToUpper(found[0]) + found.Substring(1) : string.Empty;
    }

    public class Language
    {
        public string Label { get; set; }

        public string Value { get; set; }
    }
}