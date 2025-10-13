using ISiteDefinitionRepository = EPiServer.Web.ISiteDefinitionRepository;

namespace ChildFund.Web.Infrastructure.Cms.Helpers;

public static class SiteDefinitionHelper
{
    /// <summary>
    ///     Returns the list of defined sites
    /// </summary>
    /// <returns></returns>
    public static List<Site> GetSites()
    {
        var result = new List<Site>();

        var siteDefinitionRepository = ServiceLocator.Current.GetInstance<ISiteDefinitionRepository>();
        var siteDefinitions = siteDefinitionRepository.List().ToList();
        foreach (var siteDefinition in siteDefinitions)
        {
            var siteId = siteDefinition.Id.ToString();
            var siteName = siteDefinition.Name;
            var siteUrl = siteDefinition.SiteUrl?.AbsoluteUri ?? string.Empty;
            var sitePrimaryHost = siteDefinition.GetPrimaryHost(new CultureInfo("en"));
            var siteCalculatedUrl = !string.IsNullOrEmpty(siteUrl) ? siteUrl : sitePrimaryHost.Url?.AbsoluteUri;

            result.Add(new Site
            {
                Id = siteId,
                Label = siteName,
                Url = siteCalculatedUrl,
                IsCurrent = siteDefinition.Id == SiteDefinition.Current.Id
            });
        }

        return result;
    }

    public static Uri GetPrimaryHostUri()
    {
        var siteUri = SiteDefinition.Current?.Hosts?.GetPrimaryHostDefinition()?.Url;

        if (siteUri == null)
        {
            var siteDefinitionRepository = ServiceLocator.Current.GetInstance<ISiteDefinitionRepository>();
            var site = siteDefinitionRepository.List().FirstOrDefault();

            if (site != null)
            {
                return site.Hosts.GetPrimaryHostDefinition().Url;
            }
        }

        return siteUri;
    }

    public static HostDefinition GetPrimaryHostDefinition(this IList<HostDefinition> hosts)
    {
        if (hosts == null)
        {
            throw new ArgumentNullException(nameof(hosts));
        }

        return hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Primary && !h.IsWildcardHost());
    }

    public class Site
    {
        public string Id { get; set; }

        public string Label { get; set; }

        public string Url { get; set; }

        public bool IsCurrent { get; set; }
    }
}