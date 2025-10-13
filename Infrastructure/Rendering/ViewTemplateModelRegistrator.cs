using ChildFund.Features.Shared.Pages;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;

namespace ChildFund.Infrastructure.Rendering;

[ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
public class ViewTemplateModelRegistrator : IViewTemplateModelRegistrator
{
    public const string _foundationFolder = "~/Features/Shared/Views/";

    public void Register(TemplateModelCollection viewTemplateModelRegistrator) =>
        viewTemplateModelRegistrator.Add(typeof(FoundationPageData),
            new TemplateModel
            {
                Name = "PartialPage",
                Inherit = true,
                AvailableWithoutTag = true,
                TemplateTypeCategory = TemplateTypeCategories.MvcPartialView,
                Path = $"{_foundationFolder}_Page.cshtml"
            });

    public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
    {
        // Do nothing
    }
}