using ChildFund.Web.Infrastructure.Commerce.Markets;

namespace ChildFund.Web.Features.CatalogContent.Services
{
    public interface IPricingService
    {
        IList<IPriceValue> GetPriceList(string code, MarketId marketId, PriceFilter priceFilter);
        IList<IPriceValue> GetPriceList(IEnumerable<CatalogKey> catalogKeys, MarketId marketId, PriceFilter priceFilter);
        Money? GetCurrentPrice(string code);
        Money? GetPrice(string code, MarketId marketId, Currency currency);
    }

    public class PricingService(
        IPriceService priceService,
        ICurrentMarket currentMarket,
        ICurrencyService currencyService)
        : IPricingService
    {
        public IList<IPriceValue> GetPriceList(string code, MarketId marketId, PriceFilter priceFilter)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var catalogKey = new CatalogKey(code);

            return priceService.GetPrices(marketId, DateTime.Now, catalogKey, priceFilter)
                .OrderBy(x => x.UnitPrice.Amount)
                .ToList();
        }

        public IList<IPriceValue> GetPriceList(IEnumerable<CatalogKey> catalogKeys, MarketId marketId, PriceFilter priceFilter)
        {
            if (catalogKeys == null)
            {
                throw new ArgumentNullException(nameof(catalogKeys));
            }

            if (!catalogKeys.Any())
            {
                return Enumerable.Empty<IPriceValue>().ToList();
            }

            return priceService.GetPrices(marketId, DateTime.Now, catalogKeys, priceFilter)
                .OrderBy(x => x.UnitPrice.Amount)
                .ToList();
        }

        public Money? GetCurrentPrice(string code)
        {
            var market = currentMarket.GetCurrentMarket();
            var currency = currencyService.GetCurrentCurrency();
            return GetPrice(code, market.MarketId, currency);
        }

        public Money? GetPrice(string code, MarketId marketId, Currency currency)
        {
            var prices = GetPriceList(code, marketId,
                new PriceFilter
                {
                    Currencies = new[] { currency }
                });

            return prices.Any() ? prices.First().UnitPrice : null;
        }
    }
}
