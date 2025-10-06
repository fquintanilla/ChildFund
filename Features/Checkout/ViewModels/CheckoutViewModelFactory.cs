using ChildFund.Features.Checkout.Payments;
using ChildFund.Features.Checkout.Services;
using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Infrastructure.Commerce.Customer.Services;
using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using EPiServer.Web.Routing;
using Mediachase.Commerce;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChildFund.Features.Checkout.ViewModels
{
    public class CheckoutViewModelFactory(
        LocalizationService localizationService,
        PaymentMethodViewModelFactory paymentMethodViewModelFactory,
        IAddressBookService addressBookService,
        UrlResolver urlResolver,
        IHttpContextAccessor httpContextAccessor,
        ShipmentViewModelFactory shipmentViewModelFactory,
        ICustomerService customerService)
    {
        public virtual CheckoutViewModel CreateCheckoutViewModel(ICart cart, CheckoutPage currentPage, IPaymentMethod paymentOption = null)
        {
            if (cart == null)
            {
                return CreateEmptyCheckoutViewModel(currentPage);
            }

            var currentShippingAddressId = cart.GetFirstShipment()?.ShippingAddress?.Id;
            var currentBillingAdressId = cart.GetFirstForm().Payments.FirstOrDefault()?.BillingAddress?.Id;

            var shipments = shipmentViewModelFactory.CreateShipmentsViewModel(cart).ToList();
            var useShippingAddressForBilling = shipments.Count == 1;

            var viewModel = new CheckoutViewModel(currentPage)
            {
                Shipments = shipments,
                BillingAddress = CreateBillingAddressModel(currentBillingAdressId),
                UseShippingingAddressForBilling = useShippingAddressForBilling,
                AppliedCouponCodes = cart.GetFirstForm().CouponCodes.Distinct(),
                AvailableAddresses = new List<AddressModel>(),
                ReferrerUrl = GetReferrerUrl(),
                Currency = cart.Currency,
                CurrentCustomer = customerService.GetCurrentContactViewModel(),
                Payment = paymentOption,
                AvailableSubscriptionOptions = new List<SelectListItem>()
                {
                    new SelectListItem("Monthly For A Year", "Monthly"),
                    new SelectListItem("Bi-Monthly For A Year", "2Month")
                },
                PaymentPlanSetting = new PaymentPlanSetting()
                {
                    CycleMode = Mediachase.Commerce.Orders.PaymentPlanCycle.Months,
                    IsActive = true,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow,
                    MaxCyclesCount = 12,
                    CycleLength = 1
                },
            };

            UpdatePayments(viewModel, cart);

            var availableAddresses = GetAvailableAddresses();

            if (availableAddresses.Any())
            {
                //viewModel.AvailableAddresses.Add(new AddressModel { Name = _localizationService.GetString("/Checkout/MultiShipment/SelectAddress"), AddressId = "" });

                foreach (var address in availableAddresses)
                {
                    viewModel.AvailableAddresses.Add(address);
                }
            }
            else
            {
                viewModel.AvailableAddresses.Add(new AddressModel { Name = localizationService.GetString("/Checkout/MultiShipment/NoAddressFound"), AddressId = "" });
            }

            SetDefaultShipmentAddress(viewModel, currentShippingAddressId);

            addressBookService.LoadAddress(viewModel.BillingAddress);
            PopulateCountryAndRegions(viewModel);

            return viewModel;
        }

        private void SetDefaultShipmentAddress(CheckoutViewModel viewModel, string shippingAddressId)
        {
            if (viewModel.Shipments.Count > 0)
            {
                foreach (var shipment in viewModel.Shipments)
                {
                    if (shipment.ShippingAddressType == 1)
                    {
                        viewModel.Shipments[0].Address = viewModel.AvailableAddresses.SingleOrDefault(x => x.AddressId == shippingAddressId) ??
                                                viewModel.AvailableAddresses.SingleOrDefault(x => x.ShippingDefault) ??
                                                viewModel.BillingAddress;
                    }
                }
            }
        }

        private IList<AddressModel> GetAvailableAddresses()
        {
            var addresses = addressBookService.List();
            foreach (var address in addresses.Where(x => string.IsNullOrEmpty(x.Name)))
            {
                address.Name = localizationService.GetString("/Shared/Address/DefaultAddressName");
            }

            return addresses;
        }

        private CheckoutViewModel CreateEmptyCheckoutViewModel(CheckoutPage currentPage)
        {
            return new CheckoutViewModel(currentPage)
            {
                Shipments = new List<ShipmentViewModel>(),
                AppliedCouponCodes = new List<string>(),
                AvailableAddresses = new List<AddressModel>(),
                PaymentMethodViewModels = Enumerable.Empty<PaymentMethodViewModel>(),
                UseShippingingAddressForBilling = true
            };
        }

        private void PopulateCountryAndRegions(CheckoutViewModel viewModel)
        {
            foreach (var shipment in viewModel.Shipments)
            {
                addressBookService.LoadCountriesAndRegionsForAddress(shipment.Address);
            }
        }

        private void UpdatePayments(CheckoutViewModel viewModel, ICart cart)
        {
            viewModel.PaymentMethodViewModels = paymentMethodViewModelFactory.GetPaymentMethodViewModels();
            var methodViewModels = viewModel.PaymentMethodViewModels.ToList();
            if (!methodViewModels.Any())
            {
                return;
            }

            var defaultPaymentMethod = methodViewModels.FirstOrDefault(p => p.IsDefault) ?? methodViewModels.First();
            var selectedPaymentMethod = viewModel.Payment == null ?
                defaultPaymentMethod :
                methodViewModels.Single(p => p.SystemKeyword == viewModel.Payment.SystemKeyword);

            viewModel.Payment = selectedPaymentMethod.PaymentOption;
            viewModel.Payments = methodViewModels.Where(x => cart.GetFirstForm().Payments.Any(p => p.PaymentMethodId == x.PaymentMethodId))
                .Select(x => x.PaymentOption)
                .OfType<PaymentOptionBase>()
                .ToList();

            foreach (var viewModelPayment in viewModel.Payments)
            {
                viewModelPayment.Amount =
                    new Money(
                        cart.GetFirstForm().Payments
                            .FirstOrDefault(p => p.PaymentMethodId == viewModelPayment.PaymentMethodId)?.Amount ?? 0,
                        cart.Currency);
            }

            if (!cart.GetFirstForm().
                Payments.Any())
            {
                return;
            }

            var method = methodViewModels.FirstOrDefault(
                x => x.PaymentMethodId == cart.GetFirstForm().
                         Payments.FirstOrDefault().
                         PaymentMethodId);
            if (method == null)
            {
                return;
            }

            viewModel.SelectedPayment = method.Description;
            var payment = cart.GetFirstForm().
                Payments.FirstOrDefault();
            var creditCardPayment = payment as ICreditCardPayment;
            if (creditCardPayment != null)
            {
                viewModel.SelectedPayment +=
                    $" - ({creditCardPayment.CreditCardNumber.Substring(creditCardPayment.CreditCardNumber.Length - 4)})";
            }
        }

        private AddressModel CreateBillingAddressModel(string currentBillingAdressId)
        {
            if (string.IsNullOrEmpty(currentBillingAdressId))
            {
                var preferredBillingAddress = addressBookService.GetPreferredBillingAddress();

                return new AddressModel
                {
                    AddressId = preferredBillingAddress?.Name,
                    Name = preferredBillingAddress != null ? preferredBillingAddress.Name : Guid.NewGuid().ToString(),
                };
            }
            else
            {
                return new AddressModel
                {
                    AddressId = currentBillingAdressId,
                    Name = currentBillingAdressId,
                };
            }
        }

        private string GetReferrerUrl()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var urlReferer = httpContext.Request.Headers["UrlReferrer"].ToString();
            var hostUrlReferer = string.IsNullOrEmpty(urlReferer) ? "" : new Uri(urlReferer).Host;
            if (urlReferer != null && hostUrlReferer.Equals(httpContext.Request.Host.Host.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return urlReferer;
            }

            return urlResolver.GetUrl(ContentReference.StartPage);
        }
    }
}
