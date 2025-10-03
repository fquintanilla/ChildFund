using System.Text;

namespace ChildFund.Features.Checkout
{
    public class AddToCartResult
    {
        public bool EntriesAddedToCart { get; set; }

        public IList<string> ValidationMessages { get; private set; } = new List<string>();

        public string GetComposedValidationMessage()
        {
            var allowedMessageLength = 512;
            var composedMessage = new StringBuilder();
            foreach (var message in ValidationMessages)
            {
                var messageText = message.Length + 2 < allowedMessageLength ? message : message.Substring(allowedMessageLength);
                allowedMessageLength -= message.Length;
                composedMessage.Append(messageText).Append(". ");

                if (allowedMessageLength <= 0)
                {
                    break;
                }
            }

            return composedMessage.ToString().Trim();
        }
    }
}
