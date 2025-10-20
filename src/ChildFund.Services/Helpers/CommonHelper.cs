using ChildFund.Services.Models;

namespace ChildFund.Services.Helpers
{
    public class CommonHelper
    {
        public const string USA = "840";
        public const string Canada = "124";

        public static string GetMethodOfPaymentDescription(string agpType, string cardType, string accountNumber)
        {
            var methodOfPaymentDesc = string.Empty;

            if (agpType == AGPType.CH.ToString() | agpType == AGPType.SA.ToString())
            {

                switch (agpType)
                {
                    case "CH":
                        methodOfPaymentDesc = "Checking account ending in " + accountNumber;
                        break;
                    case "SA":
                        methodOfPaymentDesc = "Savings account ending in " + accountNumber;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (cardType)
                {
                    case "VI":
                        methodOfPaymentDesc = "VISA ending in " + accountNumber;
                        break;
                    case "MC":
                        methodOfPaymentDesc = "MasterCard ending in " + accountNumber;
                        break;
                    case "DS":
                        methodOfPaymentDesc = "Discover ending in " + accountNumber;
                        break;
                    case "AE":
                        methodOfPaymentDesc = "American Express ending in " + accountNumber;
                        break;
                    default:
                        break;
                }
            }

            return methodOfPaymentDesc;
        }
    }
}
