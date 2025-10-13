namespace ChildFund.Web.Infrastructure.Cms.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        DescriptionAttribute attribute =
            (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }
}