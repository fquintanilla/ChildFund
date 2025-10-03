using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.CatalogContent.Variant
{
    public class GenericVariantController : ContentController<GenericVariant>
    {
        [HttpGet]
        public IActionResult Index(GenericVariant currentContent)
        {
            return View("~/Features/CatalogContent/Variant/Index.cshtml", currentContent);
        }
    }
}
