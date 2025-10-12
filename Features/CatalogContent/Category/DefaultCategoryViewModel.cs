using ChildFund.Features.Shared.ViewModels;

namespace ChildFund.Features.CatalogContent.Category
{
    public class DefaultCategoryViewModel : ContentViewModel<DefaultCategory>
    {
        public DefaultCategoryViewModel(DefaultCategory currentPage) : base(currentPage)
        {
        }
    }
}
