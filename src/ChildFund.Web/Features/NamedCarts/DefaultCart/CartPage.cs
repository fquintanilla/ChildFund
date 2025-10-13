using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Infrastructure.Cms.Constants;

namespace ChildFund.Web.Features.NamedCarts.DefaultCart
{
    [ContentType(DisplayName = "Cart Page",
        GUID = "4d32f8b1-7651-49db-88e2-cdcbec8ed11c",
        Description = "Page for managing cart",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-08.png")]
    public class CartPage : FoundationPageData
    {

    }
}
