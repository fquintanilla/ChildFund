using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Reflection;

namespace ChildFund.Web.Infrastructure.Display;

public class FeatureConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller) =>
        controller.Properties.Add("feature",
            GetFeatureName(controller.ControllerType));

    private string GetFeatureName(TypeInfo controllerType)
    {
        if (controllerType.FullName == null)
        {
            return string.Empty;
        }

        var tokens = controllerType.FullName.Split('.');
        if (tokens.All(t => t != "Features"))
        {
            return "";
        }

        return tokens
            .SkipWhile(t => !t.Equals("features",
                StringComparison.CurrentCultureIgnoreCase))
            .Skip(1)
            .Take(1)
            .FirstOrDefault() ?? string.Empty;
    }
}