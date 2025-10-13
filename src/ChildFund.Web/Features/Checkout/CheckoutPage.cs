using ChildFund.Web.Features.Home;
using ChildFund.Web.Features.MyAccount.OrderConfirmation;
using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Infrastructure.Cms.Constants;

namespace ChildFund.Web.Features.Checkout
{
    [ContentType(DisplayName = "Checkout Page",
        GUID = "6709cd32-7bb6-4d29-9b0b-207369799f4f",
        Description = "Checkout page",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [AvailableContentTypes(Include = [typeof(OrderConfirmationPage)], IncludeOn = [typeof(HomePage)])]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-08.png")]
    public class CheckoutPage : FoundationPageData
    {
    }
}
