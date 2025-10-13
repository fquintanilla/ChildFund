using ChildFund.Web.Features.Checkout.Services;
using ChildFund.Web.Features.Checkout.ViewModels;
using ReferenceConverter = Mediachase.Commerce.Catalog.ReferenceConverter;

namespace ChildFund.Web.Features.NamedCarts.DefaultCart
{
    public class DefaultCartController(
        ICartService cartService,
        IOrderRepository orderRepository,
        IContentLoader contentLoader,
        ReferenceConverter referenceConverter,
        CartViewModelFactory cartViewModelFactory)
        : PageController<CartPage>
    {
        private CartWithValidationIssues? _cart;

        private const string b2cMinicart = "/Features/Shared/Views/Header/_HeaderCart.cshtml";

        private CartWithValidationIssues CartWithValidationIssues => _cart ??= cartService.LoadCart(cartService.DefaultCartName, true);

        [HttpPost]
        [HttpGet]
        public async Task<ActionResult> Index(CartPage currentPage)
        {
            var messages = string.Empty;

            if (CartWithValidationIssues.Cart != null && CartWithValidationIssues.ValidationIssues.Any())
            {
                foreach (var item in CartWithValidationIssues.Cart.GetAllLineItems())
                {
                    messages = GetValidationMessages(item, CartWithValidationIssues.ValidationIssues);
                }
            }

            var viewModel = cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, currentPage);
            viewModel.Message = messages;
            return View("~/Features/NamedCarts/DefaultCart/LargeCart.cshtml", viewModel);
        }

        [HttpPost]
        [Route("/DefaultCart/AddToCart")]
        public async Task<ActionResult> AddToCart([FromBody] RequestParamsToCart param)
        {
            var warningMessage = string.Empty;

            ModelState.Clear();

            if (CartWithValidationIssues.Cart == null)
            {
                _cart = new CartWithValidationIssues
                {
                    Cart = cartService.LoadOrCreateCart(cartService.DefaultCartName),
                    ValidationIssues = new Dictionary<ILineItem, List<ValidationIssue>>()
                };
            }

            var result = cartService.AddToCart(CartWithValidationIssues.Cart, param);

            if (result.EntriesAddedToCart)
            {
                orderRepository.Save(CartWithValidationIssues.Cart);
                if (string.Equals(param.RequestFrom, "axios", StringComparison.OrdinalIgnoreCase))
                {
                    var product = "";
                    var entryLink = referenceConverter.GetContentLink(param.Code);
                    var entry = contentLoader.Get<EntryContentBase>(entryLink);
                    if (entry is BundleContent || entry is PackageContent)
                    {
                        product = entry.DisplayName;
                    }
                    else
                    {
                        var parentReference = entry.GetParentProducts().FirstOrDefault();

                        if (!ContentReference.IsNullOrEmpty(parentReference))
                        {
                            var parentProduct = contentLoader.Get<EntryContentBase>(parentReference);
                            product = parentProduct?.DisplayName;
                        }
                        else
                        {
                            product = entry.DisplayName;
                        }
                    }

                    if (result.ValidationMessages.Count > 0)
                    {
                        return Json(new ChangeCartJsonResult
                        {
                            StatusCode = result.EntriesAddedToCart ? 1 : 0,
                            CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                            Message = product + " is added to the cart successfully.\n" + result.GetComposedValidationMessage(),
                            SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
                        });
                    }

                    return Json(new ChangeCartJsonResult
                    {
                        StatusCode = result.EntriesAddedToCart ? 1 : 0,
                        CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                        Message = product + " is added to the cart successfully.",
                        SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
                    });
                }

                return MiniCartDetails();
            }

            return StatusCode(500, result.GetComposedValidationMessage());
        }

        [AcceptVerbs(new string[] { "GET", "POST" })]
        public ActionResult MiniCartDetails()
        {
            var viewModel = cartViewModelFactory.CreateMiniCartViewModel(CartWithValidationIssues.Cart);
            return PartialView(b2cMinicart, viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeCartItem([FromBody] RequestParamsToCart param) // change quantity
        {
            ModelState.Clear();

            var validationIssues = cartService.ChangeCartItem(CartWithValidationIssues.Cart, param.ShipmentId, param.Code, param.Quantity, param.Size, param.NewSize);
            orderRepository.Save(CartWithValidationIssues.Cart);
            var model = cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, null);
            if (validationIssues.Any())
            {
                foreach (var item in validationIssues.Keys)
                {
                    model.Message += GetValidationMessages(item, validationIssues);
                }
            }

            var viewModel = cartViewModelFactory.CreateLargeCartViewModel(CartWithValidationIssues.Cart, null);

            var productName = "";
            var entryLink = referenceConverter.GetContentLink(param.Code);
            productName = contentLoader.Get<EntryContentBase>(entryLink).DisplayName;

            var result = new ChangeCartJsonResult
            {
                CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                SubTotal = viewModel.Subtotal,
                Total = viewModel.Total,
                ShippingTotal = viewModel.ShippingTotal,
                TaxTotal = viewModel.TaxTotal,
                TotalDiscount = viewModel.TotalDiscount
            };

            if (validationIssues.Count > 0)
            {
                result.StatusCode = 0;
                result.Message = string.Join("\n", validationIssues.Select(x => string.Join("\n", x.Value.Select(v => v.ToString()))));
            }
            else
            {
                result.StatusCode = 1;
                result.Message = productName + " has changed from the cart.";
            }

            return Json(result);
        }

        [HttpPost]
        public ActionResult ClearCart(CartPage currentPage)
        {
            if (CartWithValidationIssues.Cart != null)
            {
                orderRepository.Delete(CartWithValidationIssues.Cart.OrderLink);
                _cart = null;
            }

            return Json(new ChangeCartJsonResult
            {
                StatusCode = 1,
                Message = " All items cleared from cart.",
                CountItems = (int)CartWithValidationIssues.Cart.GetAllLineItems().Sum(x => x.Quantity),
                SubTotal = CartWithValidationIssues.Cart.GetSubTotal()
            });
        }

        private static string GetValidationMessages(ILineItem lineItem, Dictionary<ILineItem, List<ValidationIssue>> validationIssues)
        {
            var message = string.Empty;
            foreach (var validationIssue in validationIssues)
            {
                var warning = new StringBuilder();
                warning.Append(string.Format("Line Item with code {0} ", lineItem.Code));
                validationIssue.Value.Aggregate(warning, (current, issue) => current.Append(issue).Append(", "));

                message += warning.ToString().TrimEnd(',', ' ');
            }
            return message;
        }
    }
}
