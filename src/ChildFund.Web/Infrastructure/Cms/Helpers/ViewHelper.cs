namespace ChildFund.Web.Infrastructure.Cms.Helpers;

public class ViewHelper
{
    private static readonly Lazy<IContextModeResolver> _contextModeResolver =
        new(() => ServiceLocator.Current.GetInstance<IContextModeResolver>());

    public static bool IsEditMode => _contextModeResolver.Value.CurrentMode == ContextMode.Edit;

    public static bool IsPreviewMode => _contextModeResolver.Value.CurrentMode == ContextMode.Preview;

    public static bool ValidateProperty(XhtmlString property)
    {
        return IsEditMode || property != null && !property.IsEmpty;
    }

    public static bool ValidateProperty(Url property)
    {
        return IsEditMode || property != null;
    }

    public static bool ValidateProperty(string property)
    {
        return IsEditMode || !string.IsNullOrWhiteSpace(property);
    }

    public static bool ValidateProperty(ContentArea property)
    {
        if (property == null)
        {
            return false;
        }
        return IsEditMode || property?.Items != null && property.Items.Any();
    }

    public static bool ValidateProperty(ContentReference property)
    {
        return IsEditMode || property != null;
    }

    public static bool ValidateProperty(LinkItemCollection property)
    {
        return property != null && property.Any();
    }

    public static bool ValidateProperty<T>(List<T> property)
    {
        return property != null && property.Any();
    }

    public static bool ValidateProperty<T>(IEnumerable<T> property)
    {
        return property != null && property.Any();
    }

    public static bool ValidateProperty(DateTime property)
    {
        return IsEditMode || property != DateTime.MinValue;
    }

    public static bool ValidateProperty(DateTime? property)
    {
        return IsEditMode || property != null && property != DateTime.MinValue;
    }

    public static bool ValidateProperty(LinkItem property)
    {
        return IsEditMode || property != null && !string.IsNullOrEmpty(property.Href);
    }
}