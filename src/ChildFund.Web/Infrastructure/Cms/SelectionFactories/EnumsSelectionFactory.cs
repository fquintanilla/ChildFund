namespace ChildFund.Web.Infrastructure.Cms.SelectionFactories;

public class EnumsSelectionFactory<TEnum> : ISelectionFactory
{
    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
    {
        var values = Enum.GetValues(typeof(TEnum));
        foreach (var value in values)
        {
            yield return new SelectItem { Text = GetValueName(value), Value = value };
        }
    }

    public string GetValueName(object value)
    {
        var staticName = Enum.GetName(typeof(TEnum), value);

        if (staticName == null)
        {
            return string.Empty;
        }

        var localizationPath =
            $"/property/enum/{typeof(TEnum).Name.ToLowerInvariant()}/{staticName.ToLowerInvariant()}";

        return LocalizationService.Current.TryGetString(localizationPath, out var localizedName)
            ? localizedName
            : staticName;
    }
}