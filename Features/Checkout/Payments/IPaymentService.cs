
using ChildFund.Features.Checkout.ViewModels;

namespace ChildFund.Features.Checkout.Payments
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode);
    }
}
