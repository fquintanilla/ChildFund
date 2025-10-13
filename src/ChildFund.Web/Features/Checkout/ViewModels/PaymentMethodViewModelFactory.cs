using ChildFund.Web.Features.Checkout.Payments;
using ChildFund.Web.Infrastructure.Commerce.Markets;

namespace ChildFund.Web.Features.Checkout.ViewModels
{
    public class PaymentMethodViewModelFactory(
        ICurrentMarket market,
        LanguageService languageService,
        IPaymentService paymentService,
        IEnumerable<IPaymentMethod> paymentOptions)
    {
        public IEnumerable<PaymentMethodViewModel> GetPaymentMethodViewModels()
        {
            var currentMarket = market.GetCurrentMarket().MarketId;
            var currentLanguage = languageService.GetCurrentLanguage().TwoLetterISOLanguageName;
            var availablePaymentMethods = paymentService.GetPaymentMethodsByMarketIdAndLanguageCode(currentMarket.Value, currentLanguage);

            var displayedPaymentMethods = availablePaymentMethods
                .Where(p => paymentOptions.Any(m => m.PaymentMethodId == p.PaymentMethodId))
                .Select(p => new PaymentMethodViewModel(paymentOptions.First(m => m.PaymentMethodId == p.PaymentMethodId)) { IsDefault = p.IsDefault })
                .ToList();

            return displayedPaymentMethods;
        }
    }
}
