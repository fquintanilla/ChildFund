using ChildFund.Web.Features.Shared.ViewModels;

namespace ChildFund.Web.Features.CatalogContent.Category
{
    public class DefaultCategoryViewModel : ContentViewModel<DefaultCategory>
    {
        public DefaultCategoryViewModel(DefaultCategory currentPage) : base(currentPage)
        {
        }
    }
}
