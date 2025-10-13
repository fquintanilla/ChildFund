using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChildFund.Web.Infrastructure.Cms.ModelBinders;

public class ModelBinderProvider : IModelBinderProvider
{
    private static readonly IDictionary<Type, Type> _modelBinderTypeMappings = new Dictionary<Type, Type>
    {
        { typeof(decimal), typeof(DecimalModelBinder) }, { typeof(decimal?), typeof(DecimalModelBinder) }
    };

    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (_modelBinderTypeMappings.ContainsKey(context.Metadata.ModelType))
        {
            return context.Services.GetService(_modelBinderTypeMappings[context.Metadata.ModelType]) as IModelBinder;
        }

        return null;
    }
}