namespace ChildFund.Features.CatalogContent.Variant
{
    public class GenericVariantController : ContentController<GenericVariant>
    {
        [HttpGet]
        public IActionResult Index(GenericVariant currentContent)
        {
            var model = new GenericVariantViewModel(currentContent);
            return View("~/Features/CatalogContent/Variant/Index.cshtml", model);
        }
    }
}
