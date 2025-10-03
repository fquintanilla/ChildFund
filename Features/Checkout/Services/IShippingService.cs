using ChildFund.Features.Checkout.ViewModels;
using EPiServer.Commerce.Order;
using Mediachase.Commerce.Orders;
using Mediachase.Commerce;

namespace ChildFund.Features.Checkout.Services
{
    public interface IShippingService
    {
        IList<ShippingMethodInfoModel> GetShippingMethodsByMarket(string marketid, bool returnInactive);
        ShippingMethodInfoModel GetInstorePickupModel();
        ShippingRate GetRate(IShipment shipment, ShippingMethodInfoModel shippingMethodInfoModel, IMarket currentMarket);
    }
}
