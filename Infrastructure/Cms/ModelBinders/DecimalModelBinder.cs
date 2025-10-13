using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChildFund.Infrastructure.Cms.ModelBinders;

public class DecimalModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var attemptedValue =
            bindingContext.ValueProvider.GetValue(modelName).FirstValue;

        // Depending on CultureInfo, the NumberDecimalSeparator can be "," or "."
        // Both "." and "," should be accepted, but aren't.
        var wantedSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        var alternateSeparator = wantedSeparator == "," ? "." : ",";

        if (attemptedValue != null && attemptedValue.IndexOf(wantedSeparator, StringComparison.Ordinal) == -1 &&
            attemptedValue.IndexOf(alternateSeparator, StringComparison.Ordinal) != -1)
        {
            attemptedValue =
                attemptedValue.Replace(alternateSeparator, wantedSeparator);
        }

        if (bindingContext.ModelMetadata.IsNullableValueType
            && string.IsNullOrWhiteSpace(attemptedValue))
        {
            return;
        }

        try
        {
            bindingContext.Result =
                ModelBindingResult.Success(decimal.Parse(attemptedValue ?? string.Empty, NumberStyles.Any));
        }
        catch (FormatException e)
        {
            bindingContext.Result = ModelBindingResult.Failed();
            bindingContext.ModelState.AddModelError(modelName, e.Message);
        }

        await Task.CompletedTask;
    }
}