using ChildFund.Core;
using ChildFund.Features.CatalogContent.Variant;
using ChildFund.Features.Settings;
using ChildFund.Infrastructure.Cms.Settings;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.Sponsorship
{
    public class SponsorChildBlockComponent(IChildFundClient client,
        ISettingsService settingsService,
        IContentLoader contentLoader) : AsyncBlockComponent<SponsorChildBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(SponsorChildBlock currentContent)
        {
            var kids = await client.GetRandomKidsForWebAsync();
            var take = currentContent.Count.GetValueOrDefault(6);

            var pageSettings = settingsService.GetSiteSettings<ReferencePageSettings>();
            var sponsorshipCode = contentLoader.Get<SponsorshipVariant>(pageSettings.SponsorshipVariant)?.Code;

            if (string.IsNullOrEmpty(sponsorshipCode))
            {
                throw new InvalidOperationException("Sponsorship code is not configured.");
            }

            var model = new SponsorChildViewModel(
                currentContent,
                kids?.Where(k => k is not null).Take(take).ToList() ?? [],
                sponsorshipCode);

            return View("~/Features/Sponsorship/Index.cshtml", model);
        }
    }
}
