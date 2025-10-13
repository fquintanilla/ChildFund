using ChildFund.Web.Features.Checkout.ViewModels;
using ChildFund.Web.Features.NamedCarts;

namespace ChildFund.Web.Features.Checkout.Services
{
    public interface ICartService
    {
        AddToCartResult AddToCart(ICart cart, RequestParamsToCart requestParams/*, string code, decimal quantity, string deliveryMethod, string warehouseCode, List<string> dynamicVariantOptionCodes*/);
        Dictionary<ILineItem, List<ValidationIssue>> ChangeCartItem(ICart cart, int shipmentId, string code, decimal quantity, string size, string newSize);
        void SetCartCurrency(ICart cart, Currency currency);
        Dictionary<ILineItem, List<ValidationIssue>> ValidateCart(ICart cart);
        string DefaultCartName { get; }
        CartWithValidationIssues LoadCart(string name, bool validate);
        CartWithValidationIssues LoadCart(string name, string contactId, bool validate);
        ICart LoadOrCreateCart(string name);
        ICart LoadOrCreateCart(string name, string contactId);
        Dictionary<ILineItem, List<ValidationIssue>> ChangeQuantity(ICart cart, int shipmentId, string code, decimal quantity);
        Money? GetDiscountedPrice(ICart cart, ILineItem lineItem);
        ICart CreateNewCart();
        void DeleteCart(ICart cart);
        ICart PlaceOrderToCart(IPurchaseOrder purchaseOrder, ICart cart);
    }
}