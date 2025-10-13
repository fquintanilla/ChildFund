using ChildFund.Web.Features.Checkout.ViewModels;

namespace ChildFund.Web.Features.Checkout.Payments
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode);
    }
}
