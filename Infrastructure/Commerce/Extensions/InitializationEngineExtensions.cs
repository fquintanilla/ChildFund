using EPiServer.Commerce.Routing;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace ChildFund.Infrastructure.Commerce.Extensions
{
    public static class InitializationEngineExtensions
    {
        public static void InitializeFoundationCommerce(this InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(false);
        }
    }
}
