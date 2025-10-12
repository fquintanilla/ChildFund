using ChildFund.Features.Shared.Interfaces;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace ChildFund.Infrastructure.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class RestrictedTypeInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            var uiDescriptorRegistry = context.Locate.Advanced.GetInstance<UIDescriptorRegistry>();
            var descriptors = uiDescriptorRegistry.UIDescriptors;

            var flexibleType = typeof(IRestricted);
            if (flexibleType.FullName == null) return;

            foreach (UIDescriptor descriptor in descriptors)
            {
                if (flexibleType.IsAssignableFrom(descriptor.ForType))
                {
                    descriptor.DndTypes.Add(flexibleType.FullName.ToLowerInvariant());
                }
            }
        }

        public void Uninitialize(InitializationEngine context)
        {
            //do nothing
        }
    }
}