using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.Commerce.CatalogContent.Category
{
    public class DefaultCategoryController : ContentController<DefaultCategory>
    {
        [HttpGet]
        public IActionResult Index(DefaultCategory currentContent)
        {
            return View("~/Features/Commerce/CatalogContent/Category/Index.cshtml", currentContent);
        }
    }
}
