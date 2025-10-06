using ChildFund.Features.Home;
using ChildFund.Features.MyAccount.OrderConfirmation;
using ChildFund.Features.Shared;
using ChildFund.Infrastructure;

namespace ChildFund.Features.Checkout
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
