using ChildFund.Web.Features.Checkout.Services;
using ChildFund.Web.Features.Upsell.Models;
using EPiServer.Security;

namespace ChildFund.Web.Features.Upsell.Services
{
    public class CartContextProvider(
        ICurrentMarket currentMarket,
        ICartService cartService)
        : ICartContextProvider
    {

        public CartContext Get()
        {
            var market = currentMarket.GetCurrentMarket();
            var cart = cartService.LoadOrCreateCart(cartService.DefaultCartName);
            var total = cart.GetSubTotal();

            var skus = cart?.GetAllLineItems()
                           .Select(li => li.Code)
                           .Where(c => !string.IsNullOrWhiteSpace(c))
                           .ToHashSet(StringComparer.OrdinalIgnoreCase)
                       ?? [];

            // Determine customer segments (replace with ODP or personalization)
            var segments = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (PrincipalInfo.CurrentPrincipal?.Identity?.IsAuthenticated == true)
            {
                segments.Add("LoggedInUser");
            }

            // Example: segment based on recurring items
            if (cart?.GetAllLineItems()?.Any(li =>
                    li.Properties.ContainsKey("IsRecurring") && (bool)li.Properties["IsRecurring"]) == true)
            {
                segments.Add("MonthlyDonor");
            }

            return new CartContext
            {
                CartTotal = total,
                CartSkus = skus,
                UtcNow = DateTime.UtcNow,
                CustomerSegments = segments,
                MarketId = market.MarketId.Value,
                Language = CultureInfo.CurrentUICulture.Name
            };
        }
    }
}
