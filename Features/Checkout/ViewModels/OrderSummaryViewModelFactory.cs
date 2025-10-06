using ChildFund.Infrastructure.Commerce.Markets;
using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using Mediachase.Commerce;

namespace ChildFund.Features.Checkout.ViewModels
{
    public class OrderSummaryViewModelFactory(
        IOrderGroupCalculator orderGroupCalculator,
        ICurrencyService currencyService)
    {
        public virtual OrderSummaryViewModel CreateOrderSummaryViewModel(ICart cart)
        {
            if (cart == null)
            {
                return CreateEmptyOrderSummaryViewModel();
            }

            var totals = orderGroupCalculator.GetOrderGroupTotals(cart);

            return new OrderSummaryViewModel
            {
                SubTotal = totals.SubTotal,
                CartTotal = totals.Total,
                ShippingTotal = totals.ShippingTotal,
                ShippingSubtotal = orderGroupCalculator.GetShippingSubTotal(cart),
                OrderDiscountTotal = orderGroupCalculator.GetOrderDiscountTotal(cart),
                ShippingDiscountTotal = cart.GetShippingDiscountTotal(),
                ShippingTaxTotal = totals.ShippingTotal + totals.TaxTotal,
                TaxTotal = totals.TaxTotal,
                PaymentTotal = cart.Currency.Round(totals.Total.Amount - (cart.GetFirstForm().Payments?.Sum(x => x.Amount) ?? 0)),
                OrderDiscounts = cart.GetFirstForm().Promotions.Where(x => x.DiscountType == DiscountType.Order).Select(x => new OrderDiscountViewModel
                {
                    Discount = new Money(x.SavedAmount, new Currency(cart.Currency)),
                    DisplayName = x.Description
                })
            };
        }

        private OrderSummaryViewModel CreateEmptyOrderSummaryViewModel()
        {
            var zeroAmount = new Money(0, currencyService.GetCurrentCurrency());
            return new OrderSummaryViewModel
            {
                CartTotal = zeroAmount,
                OrderDiscountTotal = zeroAmount,
                ShippingDiscountTotal = zeroAmount,
                ShippingSubtotal = zeroAmount,
                ShippingTaxTotal = zeroAmount,
                ShippingTotal = zeroAmount,
                SubTotal = zeroAmount,
                TaxTotal = zeroAmount,
                OrderDiscounts = Enumerable.Empty<OrderDiscountViewModel>(),
            };
        }
    }
}
