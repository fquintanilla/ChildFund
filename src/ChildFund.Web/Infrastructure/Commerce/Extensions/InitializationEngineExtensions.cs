using EPiServer.Commerce.Routing;
using EPiServer.Framework.Initialization;

namespace ChildFund.Web.Infrastructure.Commerce.Extensions
{
    public static class InitializationEngineExtensions
    {
        public static void InitializeFoundationCommerce(this InitializationEngine context)
        {
            CatalogRouteHelper.MapDefaultHierarchialRouter(false);
        }
    }
}
