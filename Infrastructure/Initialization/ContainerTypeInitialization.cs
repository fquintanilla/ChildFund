using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using IContainer = ChildFund.Features.Shared.Interfaces.IContainer;

namespace ChildFund.Infrastructure.Initialization;

[ModuleDependency(typeof(InitializationModule))]
public class ContainerTypeInitialization : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        var uiDescriptorRegistry = context.Locate.Advanced.GetInstance<UIDescriptorRegistry>();
        var descriptors = uiDescriptorRegistry.UIDescriptors;

        var interfaceType = typeof(IContainer);
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