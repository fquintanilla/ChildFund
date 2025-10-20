using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace ChildFund.Web.Features.Checkout.Payments
{
    public class GenericCreditCardPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            var creditCardPayment = (ICreditCardPayment)payment;
            return PaymentProcessingResult.CreateSuccessfulResult("");
        }

        public override bool ProcessPayment(Payment payment, ref string message)
        {
            var result = ProcessPayment(null, payment);
            message = result.Message;
            return result.IsSuccessful;
        }
    }
}
