using ChildFund.Features.CatalogContent.Services;
using ChildFund.Features.Checkout.ViewModels;
using ChildFund.Infrastructure.Commerce.Extensions;
using ChildFund.Infrastructure.Commerce.Markets;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Web.Routing;
using Mediachase.Commerce;

namespace ChildFund.Features.Checkout.Services
{
    public class CartItemViewModelFactory(
        IContentLoader contentLoader,
        IPricingService pricingService,
        UrlResolver urlResolver,
        ICurrencyService currencyService,
        ILineItemCalculator lineItemCalculator,
        IRelationRepository relationRepository)
    {
        public virtual CartItemViewModel CreateCartItemViewModel(ICart cart, ILineItem lineItem, EntryContentBase entry)
        {
            var basePrice = lineItem.Properties["BasePrice"] != null ? decimal.Parse(lineItem.Properties["BasePrice"].ToString()) : 0;
            var optionPrice = lineItem.Properties["OptionPrice"] != null ? decimal.Parse(lineItem.Properties["OptionPrice"].ToString()) : 0;
            var viewModel = new CartItemViewModel
            {
                Code = lineItem.Code,
                DisplayName = lineItem.DisplayName,
                ImageUrl = entry.GetAssets<IContentImage>(contentLoader, urlResolver).FirstOrDefault() ?? "",
                DiscountedPrice = GetDiscountedPrice(cart, lineItem),
                BasePrice = new Money(basePrice, currencyService.GetCurrentCurrency()),
                OptionPrice = new Money(optionPrice, currencyService.GetCurrentCurrency()),
                PlacedPrice = new Money(lineItem.PlacedPrice, currencyService.GetCurrentCurrency()),
                Quantity = lineItem.Quantity,
                Url = entry.GetUrl(relationRepository, urlResolver),
                Entry = entry,
                IsAvailable = pricingService.GetCurrentPrice(entry.Code).HasValue,
                DiscountedUnitPrice = GetDiscountedUnitPrice(cart, lineItem),
                Description = entry["Description"] != null ? entry["Description"].ToString() : ""
            };
            
            return viewModel;
        }

        private Money? GetDiscountedUnitPrice(ICart cart, ILineItem lineItem)
        {
            var discountedPrice = GetDiscountedPrice(cart, lineItem) / lineItem.Quantity;
            return discountedPrice.GetValueOrDefault().Amount < lineItem.PlacedPrice ? discountedPrice : null;
        }

        private Money? GetDiscountedPrice(ICart cart, ILineItem lineItem)
        {
            return lineItem.GetDiscountedPrice(cart.Currency, lineItemCalculator);
        }
    }
}
