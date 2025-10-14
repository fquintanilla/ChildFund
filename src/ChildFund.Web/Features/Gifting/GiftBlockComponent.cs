using ChildFund.Services.Interfaces;
using ChildFund.Web.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildFund.Web.Features.Gifting
{
    public class GiftBlockComponent(
        IDonorPortalClient client,
        ISettingsService settingsService,
        IContentLoader contentLoader
    ) : AsyncBlockComponent<GiftBlock>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(GiftBlock currentContent)
        {
            var vm = new GiftBlockViewModel();

            // Build Occasion dropdown from the variants selected on the block
            if (currentContent?.Variants?.FilteredItems is not null)
            {
                foreach (var caItem in currentContent.Variants.FilteredItems)
                {
                    if (contentLoader.TryGet(caItem.ContentLink, out VariationContent variation))
                    {
                        vm.OccasionOptions.Add(new SelectListItem
                        {
                            Text = variation.DisplayName ?? variation.Name,
                            Value = variation.Code // needed for AddToCart
                        });
                    }
                }
            }

            var children = await client.GetLteChildrenByContactId("847166170");

            vm.RecipientOptions = (children ?? [])
                .Where(c => c is not null)
                .OrderBy(c => c.ChildName)
                .Select(c => new SelectListItem
                {
                    Text = c.ChildName,
                    Value = c.ChildNbr.ToString()
                })
                .ToList();

            return View("~/Features/Gifting/Index.cshtml", vm);
        }
    }
}
