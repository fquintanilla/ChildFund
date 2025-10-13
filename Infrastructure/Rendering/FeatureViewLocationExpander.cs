using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ChildFund.Infrastructure.Rendering;

public class FeatureViewLocationExpander : IViewLocationExpander
{
    private const string _feature = "feature";

    private readonly List<string> _viewLocationFormats = new()
    {
        "/Features/Shared/{0}.cshtml",
        "/Features/Blocks/{0}.cshtml",
        "/Features/Blocks/{1}/{0}.cshtml",
        "/Features/Shared/Views/{0}.cshtml",
        "/Features/Shared/Views/{1}/{0}.cshtml",
        "/Features/Shared/Views/Header/{0}.cshtml",
        "/Cms/Views/{1}/{0}.cshtml",
        "/Features/{3}/{1}/{0}.cshtml",
        "/Features/{3}/{0}.cshtml",
        "/Features/Shared/Views/ElementBlocks/{0}.cshtml",
        "/FormsViews/Views/ElementBlocks/{0}.cshtml"
    };

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (viewLocations == null)
        {
            throw new ArgumentNullException(nameof(viewLocations));
        }

        return GenerateViewLocations(context, viewLocations);
    }

    public void PopulateValues(ViewLocationExpanderContext context)
    {
        var controllerActionDescriptor = context.ActionContext?.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor == null || !controllerActionDescriptor.Properties.ContainsKey(_feature))
        {
            return;
        }

        context.Values[_feature] = controllerActionDescriptor.Properties[_feature].ToString();
    }

    private IEnumerable<string> GenerateViewLocations(ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        if (context.ActionContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor &&
            controllerActionDescriptor.Properties.ContainsKey("feature"))
        {
            var featureName = controllerActionDescriptor.Properties[_feature] as string;
            foreach (var item in ExpandViewLocations(_viewLocationFormats.Union(viewLocations), featureName))
            {
                yield return item;
            }
        }
        else
        {
            foreach (var location in viewLocations)
            {
                yield return location;
            }
        }
    }

    private IEnumerable<string> ExpandViewLocations(IEnumerable<string> viewLocations, string featureName)
    {
        foreach (var location in viewLocations)
        {
            yield return location.Replace("{3}", featureName);
        }
    }
}