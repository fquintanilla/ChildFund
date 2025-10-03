using ChildFund.Infrastructure.Commerce.Extensions;
using EPiServer.Commerce.Internal.Migration;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace ChildFund.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var manager = context.Locate.Advanced.GetInstance<MigrationManager>();
            if (manager.SiteNeedsToBeMigrated())
            {
                manager.Migrate();
            }

            context.InitializeFoundationCommerce();
        }

        public void Uninitialize(InitializationEngine context)
        {
            
        }

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            
        }
    }
}
