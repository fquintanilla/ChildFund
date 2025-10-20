using Mediachase.Commerce.Orders;
using Mediachase.Commerce.Plugins.Payment;

namespace ChildFund.Web.Features.Checkout.Payments 
{
    public class AccountPaymentGateway : AbstractPaymentGateway, IPaymentPlugin
    {
        public PaymentProcessingResult ProcessPayment(IOrderGroup orderGroup, IPayment payment)
        {
            // Add your custom payment processing logic here if needed.
            if (string.IsNullOrEmpty(payment.Properties["AccountNumber"]?.ToString()))
            {
                return PaymentProcessingResult.CreateUnsuccessfulResult("Invalid account number.");
            }

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
