using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.Commerce.CatalogContent.Variant
{
    public class GenericVariantController : ContentController<GenericVariant>
    {
        [HttpGet]
        public IActionResult Index(GenericVariant currentContent)
        {
            return View("~/Features/Commerce/CatalogContent/Variant/Index.cshtml", currentContent);
        }
    }
}
