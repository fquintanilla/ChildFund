using ChildFund.Web.Infrastructure.Commerce.Markets;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Orders.Managers;

namespace ChildFund.Web.Features.Checkout.Payments
{
    public class AccountPaymentOption(
        LocalizationService localizationService,
        IOrderGroupFactory orderGroupFactory,
        ICurrentMarket currentMarket,
        LanguageService languageService,
        IPaymentService paymentService)
        : PaymentOptionBase(localizationService, orderGroupFactory, currentMarket, languageService, paymentService)
    {
        public override string SystemKeyword => "Account";

        public string AccountNumber { get; set; } = string.Empty;

        public override IPayment CreatePayment(decimal amount, IOrderGroup orderGroup)
        {
            var implementationClassName = PaymentManager.GetPaymentMethod(base.PaymentMethodId, false).PaymentMethod[0]
                .PaymentImplementationClassName;

            var type = Type.GetType(implementationClassName);
            var payment = type == null 
                ? orderGroup.CreatePayment(OrderGroupFactory) 
                : orderGroup.CreatePayment(OrderGroupFactory, type);

            payment.PaymentMethodId = PaymentMethodId;
            payment.PaymentMethodName = SystemKeyword;
            payment.Amount = amount;
            payment.PaymentType = PaymentType.Other;

            // Set custom properties
            payment.Properties["AccountNumber"] = this.AccountNumber;

            return payment;
        }

        public override bool ValidateData()
        {
            // keep it simple; add your own rules if needed
            return !string.IsNullOrWhiteSpace(AccountNumber);
        }
    }
}
