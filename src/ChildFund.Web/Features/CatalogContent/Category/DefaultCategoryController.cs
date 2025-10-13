namespace ChildFund.Web.Features.CatalogContent.Category
{
    public class DefaultCategoryController : ContentController<DefaultCategory>
    {
        [HttpGet]
        public IActionResult Index(DefaultCategory currentContent)
        {
            var model = new DefaultCategoryViewModel(currentContent);
            return View("~/Features/CatalogContent/Category/Index.cshtml", model);
        }
    }
}
