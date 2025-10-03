using ChildFund.Features.Checkout.Services;
using ChildFund.Features.Checkout.ViewModels;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Web.Mvc;
using Mediachase.Commerce.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.NamedCarts.DefaultCart
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
    }
}
