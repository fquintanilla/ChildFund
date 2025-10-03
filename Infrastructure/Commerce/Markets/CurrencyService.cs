using ChildFund.Infrastructure.Cms;
using Mediachase.Commerce;

namespace ChildFund.Infrastructure.Commerce.Markets
{
    public class CurrencyService(ICurrentMarket currentMarket, ICookieService cookieService) : ICurrencyService
    {
        private const string CurrencyCookie = "Currency";

        private IMarket CurrentMarket => currentMarket.GetCurrentMarket();

        public IEnumerable<Currency> GetAvailableCurrencies() => CurrentMarket.Currencies;

        public virtual Currency GetCurrentCurrency()
        {
            return TryGetCurrency(cookieService.Get(CurrencyCookie), out var currency)
                ? currency
                : CurrentMarket.DefaultCurrency;
        }

        public bool SetCurrentCurrency(string currencyCode)
        {
            if (!TryGetCurrency(currencyCode, out _))
            {
                return false;
            }

            cookieService.Set(CurrencyCookie, currencyCode);

            return true;
        }

        private bool TryGetCurrency(string currencyCode, out Currency currency)
        {
            var result = GetAvailableCurrencies()
                .Where(x => x.CurrencyCode == currencyCode)
                .Cast<Currency?>()
                .FirstOrDefault();

            if (result.HasValue)
            {
                currency = result.Value;
                return true;
            }

            currency = null;
            return false;
        }
    }
}
