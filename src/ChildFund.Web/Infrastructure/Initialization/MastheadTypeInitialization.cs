using ChildFund.Web.Features.Shared.Interfaces;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace ChildFund.Web.Infrastructure.Initialization;

[ModuleDependency(typeof(InitializationModule))]
public class MastheadTypeInitialization : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        var uiDescriptorRegistry = context.Locate.Advanced.GetInstance<UIDescriptorRegistry>();
        var descriptors = uiDescriptorRegistry.UIDescriptors;

        var interfaceType = typeof(IMasthead);
        if (interfaceType.FullName == null) return;

        foreach (var descriptor in descriptors)
        {
            if (interfaceType.IsAssignableFrom(descriptor.ForType))
            {
                descriptor.DndTypes.Add(interfaceType.FullName.ToLowerInvariant());
            }
        }
    }

    public void Uninitialize(InitializationEngine context)
    {
        //do nothing
    }
}