using ChildFund.Features.Checkout;
using ChildFund.Infrastructure.Commerce.Markets;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.Web.Routing;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using Mediachase.Commerce;
using System.Globalization;
using ChildFund.Infrastructure.Commerce.Extensions;

namespace ChildFund.Features.CatalogContent.Services
{
    public interface IProductService
    {
        ProductTileViewModel GetProductTileViewModel(EntryContentBase entry);
        IEnumerable<ProductTileViewModel> GetProductTileViewModels(IEnumerable<ContentReference> entryLinks);
        IEnumerable<VariationContent> GetVariants(ProductContent currentContent);
    }

    public class ProductService(
        IContentLoader contentLoader,
        UrlResolver urlResolver,
        IRelationRepository relationRepository,
        ICurrentMarket currentMarketService,
        ICurrencyService currencyService,
        LanguageService languageService,
        ICurrentMarket currentMarket,
        IPromotionService promotionService)
        : IProductService
    {
        private readonly CultureInfo _preferredCulture = ContentLanguage.PreferredCulture;
        private readonly FilterPublished _filterPublished = new();

        public IEnumerable<VariationContent> GetVariants(ProductContent currentContent) => GetAvailableVariants(currentContent.GetVariants(relationRepository));

        public IEnumerable<ProductTileViewModel> GetProductTileViewModels(IEnumerable<ContentReference> entryLinks)
        {
            var language = languageService.GetCurrentLanguage();
            var contentItems = contentLoader.GetItems(entryLinks, language);
            return contentItems.OfType<EntryContentBase>().Select(x => x.GetProductTileViewModel(currentMarket.GetCurrentMarket(), currencyService.GetCurrentCurrency()));
        }

        public virtual ProductTileViewModel GetProductTileViewModel(EntryContentBase entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (entry is PackageContent)
            {
                return CreateProductViewModelForEntry((PackageContent)entry);
            }

            if (entry is ProductContent)
            {
                var product = (ProductContent)entry;
                var variant = GetAvailableVariants(product.GetVariants()).FirstOrDefault();

                return CreateProductViewModelForVariant(product, variant);
            }

            if (entry is VariationContent)
            {
                ProductContent product = null;
                var parentLink = entry.GetParentProducts(relationRepository).SingleOrDefault();
                if (!ContentReference.IsNullOrEmpty(parentLink))
                {
                    product = contentLoader.Get<ProductContent>(parentLink);
                }

                return CreateProductViewModelForVariant(product, (VariationContent)entry);
            }

            throw new ArgumentException("BundleContent is not supported", nameof(entry));
        }

        private IEnumerable<VariationContent> GetAvailableVariants(IEnumerable<ContentReference> contentLinks)
        {
            return contentLoader.GetItems(contentLinks, _preferredCulture)
                                                            .OfType<VariationContent>()
                                                            .Where(v => v.IsAvailableInCurrentMarket(currentMarketService) && !_filterPublished.ShouldFilter(v));
        }

        private ProductTileViewModel CreateProductViewModelForEntry(EntryContentBase entry)
        {
            var market = currentMarketService.GetCurrentMarket();
            var currency = currencyService.GetCurrentCurrency();
            var originalPrice = PriceCalculationService.GetSalePrice(entry.Code, market.MarketId, market.DefaultCurrency);
            Money? discountedPrice;

            if (originalPrice?.UnitPrice == null || originalPrice.UnitPrice.Amount == 0)
            {
                originalPrice = new PriceValue() { UnitPrice = new Money(0, market.DefaultCurrency) };
                discountedPrice = null;
            }
            else
            {
                discountedPrice = GetDiscountPrice(entry, market, currency, originalPrice.UnitPrice);
            }

            var image = entry.GetAssets<IContentImage>(contentLoader, urlResolver).FirstOrDefault() ?? "";
            
            return new ProductTileViewModel
            {
                Code = entry.Code,
                DisplayName = entry.DisplayName,
                PlacedPrice = originalPrice.UnitPrice,
                DiscountedPrice = discountedPrice,
                ImageUrl = image,
                Url = entry.GetUrl(),
                IsAvailable = originalPrice.UnitPrice != null && originalPrice.UnitPrice.Amount > 0
            };
        }

        private ProductTileViewModel CreateProductViewModelForVariant(ProductContent product, VariationContent variant)
        {
            if (variant == null)
            {
                return null;
            }

            var viewModel = CreateProductViewModelForEntry(variant);
            if (product == null)
            {
                return viewModel;
            }
            
            return viewModel;
        }

        private Money GetDiscountPrice(EntryContentBase entry, IMarket market, Currency currency, Money originalPrice)
        {
            var discountedPrice = promotionService.GetDiscountPrice(new CatalogKey(entry.Code), market.MarketId, currency);
            return discountedPrice?.UnitPrice ?? originalPrice;
        }
    }
}
