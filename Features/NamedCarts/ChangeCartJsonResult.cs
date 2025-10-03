namespace ChildFund.Features.NamedCarts
{
    public class ChangeCartJsonResult
    {
    }

    public class RequestParamsToCart
    {
        public string Code { get; set; }
        public int ShipmentId { get; set; }
        public decimal Quantity { get; set; } = 1;
        public string Size { get; set; } = null;
        public string NewSize { get; set; } = null;

        // for Add to cart
        public string Store { get; set; } = "delivery";
        public string SelectedStore { get; set; } = "";
        public string RequestFrom { get; set; } = "";

        // for SharedCart 
        public string OrganizationId { get; set; }

        // for Checkout Separate shipment
        public int ToShipmentId { get; set; }
        public string DeliveryMethodId { get; set; }

        // for DynamicProduct 
        public List<string> DynamicCodes { get; set; }
    }
}
