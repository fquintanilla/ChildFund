using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildFund.Web.Features.Gifting
{
    public class GiftBlockViewModel
    {
        public string PaymentFrequencyValue { get; set; }
        public List<SelectListItem> OccasionOptions { get; set; } = [];
        public List<SelectListItem> RecipientOptions { get; set; } = [];
    }
}
