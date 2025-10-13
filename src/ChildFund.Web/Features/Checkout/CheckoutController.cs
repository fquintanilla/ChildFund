using ChildFund.Web.Core.Settings;
using ChildFund.Web.Features.Checkout.Payments;
using ChildFund.Web.Features.Checkout.Services;
using ChildFund.Web.Features.Checkout.ViewModels;
using ChildFund.Web.Features.MyAccount.AddressBook;
using ChildFund.Web.Infrastructure.Cms.Settings;
using ChildFund.Web.Infrastructure.Commerce;
using ChildFund.Web.Infrastructure.Commerce.Customer.Services;
using ChildFund.Web.Infrastructure.Helpers;

namespace ChildFund.Web.Features.Checkout
{
    public class CheckoutController(ICartService cartService,
        OrderSummaryViewModelFactory orderSummaryViewModelFactory,
        IAddressBookService addressBookService,
        CheckoutViewModelFactory checkoutViewModelFactory,
        CheckoutService checkoutService,
        IContentLoader contentLoader,
        ISettingsService settingsService,
        IOrderRepository orderRepository,
        ICustomerService customerContext) : PageController<CheckoutPage>
    {
        private CartWithValidationIssues? _cart;

        [HttpGet]
        public IActionResult Index(CheckoutPage currentPage, int? isGuest)
        {
            if (CartIsNullOrEmpty())
            {
                return View("~/Features/Checkout/EmptyCart.cshtml", new CheckoutMethodViewModel(currentPage));
            }


            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            viewModel.BillingAddress = addressBookService.ConvertToModel(CartWithValidationIssues.Cart.GetFirstForm()?.Payments.FirstOrDefault()?.BillingAddress);
            addressBookService.LoadAddress(viewModel.BillingAddress);

            var shipmentBillingTypes = TempData.Get<List<KeyValuePair<string, int>>>("ShipmentBillingTypes");

            if (shipmentBillingTypes != null && shipmentBillingTypes.Any(x => x.Key == "Billing"))
            {
                viewModel.BillingAddressType = 0;
            }
            else
            {
                if (viewModel.Shipments.Count == 1)
                {
                    viewModel.BillingAddressType = 2;
                }
                else if (HttpContext.User.Identity.IsAuthenticated)
                {
                    viewModel.BillingAddressType = 1;
                }
                else
                {
                    viewModel.BillingAddressType = 0;
                }
            }

            var shippingAddressType = HttpContext.User.Identity.IsAuthenticated ? 1 : 0;
            for (var i = 0; i < viewModel.Shipments.Count; i++)
            {
                if (shipmentBillingTypes != null && shipmentBillingTypes.Where(x => x.Key == "Shipment").Any(x => x.Value == i))
                {
                    viewModel.Shipments[i].ShippingAddressType = 0;
                }
                else
                {
                    if (string.IsNullOrEmpty(viewModel.Shipments[i].Address.AddressId))
                    {
                        viewModel.Shipments[i].ShippingAddressType = shippingAddressType;
                    }
                    else
                    {
                        viewModel.Shipments[i].ShippingAddressType = 1;
                    }
                }
            }

            if (TempData[Constant.ErrorMessages] != null)
            {
                ViewBag.ErrorMessages = (string)TempData[Constant.ErrorMessages];
            }

            var tempDataState = TempData.Get<List<KeyValuePair<string, string>>>("ModelState");
            if (tempDataState != null)
            {
                foreach (var e in tempDataState)
                {
                    ViewData.ModelState.AddModelError(e.Key, e.Value);
                }
            }

            return View("~/Features/Checkout/Checkout.cshtml", viewModel);
        }

        [HttpGet]
        public IActionResult PlaceOrder(CheckoutPage currentPage)
        {
            var viewModel = CreateCheckoutViewModel(currentPage);
            viewModel.OrderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return View("~/Features/Checkout/PlaceOrder.cshtml", viewModel);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutPage currentPage, [FromForm] CheckoutViewModel checkoutViewModel)
        {
            ModelState.Clear();

            // store the shipment indexes and billing address properties if they are invalid when run TryValidateModel
            // format: key = Shipment | Billing
            var errorTypes = new List<KeyValuePair<string, int>>();

            // shipping information
            UpdateShipmentAddress(checkoutViewModel, errorTypes);

            // billing address
            UpdatePaymentAddress(checkoutViewModel, errorTypes);
            orderRepository.Save(CartWithValidationIssues.Cart);

            if (!ModelState.IsValid)
            {
                var stateValues = new List<KeyValuePair<string, string>>();
                stateValues.AddRange(ModelState.Select(x => new KeyValuePair<string, string>(x.Key, x.Value.Errors.FirstOrDefault().ErrorMessage)));
                TempData.Set("ModelState", stateValues);
                TempData.Set("ShipmentBillingTypes", errorTypes);
                //return RedirectToAction("Index");
                return Json(new { Status = false, Message = stateValues.Select(x => $"{x.Key},{x.Value}") });
            }

            try
            {
                var purchaseOrder = checkoutService.PlaceOrder(CartWithValidationIssues.Cart, ModelState, checkoutViewModel);
                if (purchaseOrder == null)
                {
                    TempData[Constant.ErrorMessages] = "There is no payment was processed";
                    return Json(new { Status = false, Message = "There is no payment was processed" });
                    //return RedirectToAction("Index");
                }

                if (checkoutViewModel.BillingAddressType == 0)
                {
                    addressBookService.Save(checkoutViewModel.BillingAddress);
                }

                foreach (var shipment in checkoutViewModel.Shipments)
                {
                    if (shipment.ShippingAddressType == 0)
                    {
                        addressBookService.Save(shipment.Address);
                    }
                }

                if (HttpContext.User.Identity.IsAuthenticated)
                {
                    var contact = customerContext.GetCurrentContact().Contact;
                    var organization = contact.ContactOrganization;
                    if (organization != null)
                    {
                        purchaseOrder.Properties[Constant.Customer.CustomerFullName] = contact.FullName;
                        purchaseOrder.Properties[Constant.Customer.CustomerEmailAddress] = contact.Email;
                        purchaseOrder.Properties[Constant.Customer.CurrentCustomerOrganization] = organization.Name;
                        orderRepository.Save(purchaseOrder);
                    }
                }
                checkoutViewModel.CurrentContent = currentPage;
                var confirmationSentSuccessfully = await checkoutService.SendConfirmation(checkoutViewModel, purchaseOrder);

                string redirectURL = checkoutService.BuildRedirectionUrl(checkoutViewModel, purchaseOrder, confirmationSentSuccessfully);
                return Json(new { Status = true, RedirectUrl = redirectURL });
                //return Redirect(redirectURL);
            }
            catch (Exception e)
            {
                TempData[Constant.ErrorMessages] = e.Message;
                //return RedirectToAction("Index");
                return Json(new { Status = false, e.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddAddress(CheckoutPage? currentPage, AddressModel viewModel, string? returnUrl)
        {
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                ModelState.AddModelError("Address.Name", "Name is required");
            }

            if (!addressBookService.CanSave(viewModel))
            {
                ModelState.AddModelError("Address.Name", "An address with the same name already exists");
            }

            if (!ModelState.IsValid)
            {
                var error = ModelState.Select(x =>
                {
                    if (x.Value.Errors.Count > 0)
                    {
                        return x.Key + ": " + string.Join(" ", x.Value.Errors.Select(y => y.ErrorMessage)) + "</br>";
                    }
                    return "";
                });

                return Json(new { Status = false, Message = error });
            }

            addressBookService.Save(viewModel);
            return Json(new { Status = true, RedirectUrl = returnUrl });
        }

        public void UpdateShipmentAddress(CheckoutViewModel checkoutViewModel, List<KeyValuePair<string, int>> errorTypes)
        {
            var content = settingsService.GetSiteSettings<ReferencePageSettings>().CheckoutPage;
            var checkoutPage = contentLoader.Get<PageData>(content) as CheckoutPage;
            var viewModel = CreateCheckoutViewModel(checkoutPage);
            if (!checkoutViewModel.UseShippingingAddressForBilling)
            {
                for (var i = 0; i < checkoutViewModel.Shipments.Count; i++)
                {
                    if (checkoutViewModel.Shipments[i].ShippingAddressType == 0)
                    {
                        var addressName = checkoutViewModel.Shipments[i].Address.FirstName + " " + checkoutViewModel.Shipments[i].Address.LastName;
                        checkoutViewModel.Shipments[i].Address.AddressId = null;
                        checkoutViewModel.Shipments[i].Address.Name = addressName + " " + DateTime.Now.ToString();
                        viewModel.Shipments[i].Address = checkoutViewModel.Shipments[i].Address;

                        if (!TryValidateModel(checkoutViewModel.Shipments[i].Address, "Shipments[" + i + "].Address"))
                        {
                            errorTypes.Add(new KeyValuePair<string, int>("Shipment", i));
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(checkoutViewModel.Shipments[i].Address.AddressId))
                        {
                            viewModel.Shipments[i].ShippingAddressType
                                = 1;
                            ModelState.AddModelError("Shipments[" + i + "].Address.AddressId", "Address is required.");
                        }

                        addressBookService.LoadAddress(checkoutViewModel.Shipments[i].Address);
                        viewModel.Shipments[i].Address = checkoutViewModel.Shipments[i].Address;
                    }
                }
            }

            checkoutService.UpdateShippingAddresses(CartWithValidationIssues.Cart, viewModel);
        }

        [HttpPost]
        public IActionResult UpdatePayment(CheckoutPage currentPage, [FromForm] CheckoutViewModel viewModel)
        {

            var paymentOption = viewModel.SystemKeyword.GetPaymentMethod();
            if (paymentOption == null || !paymentOption.ValidateData())
            {
                return View("~/Features/Checkout/Index.cshtml", viewModel);
            }

            viewModel.Payment = paymentOption;
            viewModel.OrderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            checkoutService.CreateAndAddPaymentToCart(CartWithValidationIssues.Cart, viewModel);
            orderRepository.Save(CartWithValidationIssues.Cart);

            var model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                model.BillingAddressType = 1;
            }
            else
            {
                model.BillingAddressType = 0;
            }
            return PartialView("~/Features/Checkout/_AddPayment.cshtml", model);
        }

        [HttpPost]
        public IActionResult RemovePayment(CheckoutPage currentPage, [FromBody] CheckoutViewModel viewModel)
        {
            var paymentOption = viewModel.SystemKeyword.GetPaymentMethod();
            if (paymentOption == null || !paymentOption.ValidateData())
            {
                return View("~/Features/Checkout/Index.cshtml", viewModel);
            }

            viewModel.Payment = paymentOption;
            checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, viewModel);
            orderRepository.Save(CartWithValidationIssues.Cart);

            var model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                model.BillingAddressType = 1;
            }
            else
            {
                model.BillingAddressType = 0;
            }
            return PartialView("~/Features/Checkout/_AddPayment.cshtml", model);
        }

        public void UpdatePaymentAddress(CheckoutViewModel viewModel, List<KeyValuePair<string, int>> errorTypes)
        {
            var orderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            var isMissingPayment = !CartWithValidationIssues.Cart.Forms.SelectMany(x => x.Payments).Any();
            if (isMissingPayment || orderSummary.PaymentTotal != 0)
            {
                if (viewModel.BillingAddressType == 1)
                {
                    if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
                    {
                        ModelState.AddModelError("BillingAddress.AddressId", "Address is required.");
                    }
                }

                if (isMissingPayment)
                {
                    ModelState.AddModelError("SelectedPayment", "PaymentRequired");
                    return;
                }

                if (orderSummary.PaymentTotal != 0)
                {
                    ModelState.AddModelError("PaymentTotal", "PaymentTotal is invalid.");
                    return;
                }
            }

            if (viewModel.BillingAddressType == 1)
            {
                if (string.IsNullOrEmpty(viewModel.BillingAddress.AddressId))
                {
                    ModelState.AddModelError("BillingAddress.AddressId", "Address is required.");
                    return;
                }

                addressBookService.LoadAddress(viewModel.BillingAddress);
            }
            else if (viewModel.BillingAddressType == 2)
            {
                viewModel.BillingAddress = viewModel.Shipments.FirstOrDefault()?.Address;
                if (viewModel.BillingAddress == null)
                {
                    ModelState.AddModelError("BillingAddress.AddressId", "Shipping address is required.");
                    return;
                }
            }
            else
            {
                var addressName = viewModel.BillingAddress.FirstName + " " + viewModel.BillingAddress.LastName;
                viewModel.BillingAddress.AddressId = null;
                viewModel.BillingAddress.Name = addressName + " " + DateTime.Now.ToString();

                if (!TryValidateModel(viewModel.BillingAddress, "BillingAddress"))
                {
                    errorTypes.Add(new KeyValuePair<string, int>("Billing", 1));
                }
            }

            foreach (var payment in CartWithValidationIssues.Cart.GetFirstForm().Payments)
            {
                payment.BillingAddress = addressBookService.ConvertToAddress(viewModel.BillingAddress, CartWithValidationIssues.Cart);
            }
        }

        public IActionResult ChangeCartItem(CheckoutPage currentPage, string code, int quantity, int shipmentId = -1)
        {
            var result = cartService.ChangeQuantity(CartWithValidationIssues.Cart, shipmentId, code, quantity);
            var model = CreateCheckoutViewModel(currentPage);

            foreach (var payment in model.Payments)
            {
                var paymentViewmodel = new CheckoutViewModel
                {
                    Payment = payment
                };
                checkoutService.RemovePaymentFromCart(CartWithValidationIssues.Cart, paymentViewmodel);
            }
            orderRepository.Save(CartWithValidationIssues.Cart);
            model = CreateCheckoutViewModel(currentPage);
            model.OrderSummary = orderSummaryViewModelFactory.CreateOrderSummaryViewModel(CartWithValidationIssues.Cart);
            return PartialView("~/Features/Checkout/_AddPayment.cshtml", model);
        }

        private CheckoutViewModel CreateCheckoutViewModel(CheckoutPage currentPage, IPaymentMethod paymentOption = null) => checkoutViewModelFactory.CreateCheckoutViewModel(CartWithValidationIssues.Cart, currentPage, paymentOption);

        private CartWithValidationIssues CartWithValidationIssues => _cart ??= cartService.LoadCart(cartService.DefaultCartName, true);

        private bool CartIsNullOrEmpty() => CartWithValidationIssues.Cart == null || !CartWithValidationIssues.Cart.GetAllLineItems().Any();
    }
}
