using ChildFund.Features.Shared;
using ChildFund.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ChildFund.Features.MyAccount.OrderConfirmation
{
    [ContentType(DisplayName = "Order Confirmation Page",
        GUID = "04285260-47be-4ecf-9118-558d6c88d3c0",
        Description = "Page to show when successful checkout",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [AvailableContentTypes(Availability = Availability.None)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-08.png")]
    public class OrderConfirmationPage : FoundationPageData
    {

    }
}
