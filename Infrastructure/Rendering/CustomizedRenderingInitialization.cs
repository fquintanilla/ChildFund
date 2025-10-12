using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc.Html;

namespace ChildFund.Infrastructure.Rendering;

/// <summary>
///     Module for customizing templates and rendering.
/// </summary>
[ModuleDependency(typeof(InitializationModule))]
public class CustomizedRenderingInitialization : IConfigurableModule
{
    public void ConfigureContainer(ServiceConfigurationContext context) =>
        //Implementations for custom interfaces can be registered here.
        context.ConfigurationComplete += (_, _) =>
        {
            //Register custom implementations that should be used in favor of the default implementations
            context.Services.AddTransient<ContentAreaRenderer, CustomContentAreaRenderer>();
        };

    public void Initialize(InitializationEngine context) =>
        context.Locate.Advanced.GetInstance<ITemplateResolverEvents>().TemplateResolved +=
            ViewTemplateModelRegistrator.OnTemplateResolved;

    public void Uninitialize(InitializationEngine context) =>
        context.Locate.Advanced.GetInstance<ITemplateResolverEvents>().TemplateResolved -=
            ViewTemplateModelRegistrator.OnTemplateResolved;
}