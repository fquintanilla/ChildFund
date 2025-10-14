using Mediachase.Commerce.Customers;

namespace ChildFund.Web.Infrastructure.Commerce.Pricing
{
    /// <summary>
    /// Decorates the default placed price processor so lines marked with IsCustomPrice keep
    /// their user-entered PlacedPrice instead of being overwritten by catalog prices.
    /// </summary>
    public class CustomPlacedPriceProcessor(IPlacedPriceProcessor inner) : IPlacedPriceProcessor
    {
        public const string IsCustomPriceKey = Constant.LineItemFields.IsCustomPrice;
        
        private static bool IsCustom(ILineItem? lineItem)
        {
            if (lineItem == null) return false;

            if (lineItem.Properties.ContainsKey(IsCustomPriceKey))
            {
                var val = lineItem.Properties[IsCustomPriceKey];
                return val is bool b && b && lineItem.PlacedPrice > 0m;
            }

            return false;
        }

        /// <summary>
        /// If the line is flagged as custom, do NOT change PlacedPrice and report success.
        /// Otherwise, delegate to the default processor.
        /// </summary>
        public bool UpdatePlacedPrice(
            ILineItem lineItem,
            CustomerContact customerContact,
            MarketId marketId,
            Currency currency,
            Action<ILineItem, ValidationIssue> onValidationError)
        {
            if (IsCustom(lineItem))
            {
                // Price already set by UI; keep it and consider it valid.
                // If you want extra validation (e.g., min amount), do it here and
                // call onValidationError(lineItem, ValidationIssue.PlacedPriceInvalid);
                return true;
            }

            return inner.UpdatePlacedPrice(lineItem, customerContact, marketId, currency, onValidationError);
        }

        public Money? GetPlacedPrice(
            EntryContentBase entry,
            decimal quantity,
            CustomerContact customerContact,
            MarketId marketId,
            Currency currency)
        {
            return inner.GetPlacedPrice(entry, quantity, customerContact, marketId, currency);
        }
    }
}
