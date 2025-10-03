using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.CatalogContent.Category
{
    public class DefaultCategoryController : ContentController<DefaultCategory>
    {
        [HttpGet]
        public IActionResult Index(DefaultCategory currentContent)
        {
            return View("~/Features/CatalogContent/Category/Index.cshtml", currentContent);
        }
    }
}
