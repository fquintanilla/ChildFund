using ChildFund.Infrastructure.Cms.Extensions;

namespace ChildFund.Infrastructure.Cms.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MaxHtmlLengthAttribute : ValidationAttribute
{
    private readonly int _max;

    public MaxHtmlLengthAttribute(int max) => _max = max;

    public override string FormatErrorMessage(string name)
    {
        if (string.IsNullOrEmpty(ErrorMessage))
        {
            ErrorMessage = "{0} must have {1} characters max.";
        }

        return string.Format(CultureInfo.InvariantCulture, ErrorMessageString, name, _max);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return null;
        }

        //Ignore other types
        if (value is not XhtmlString xhtmlString)
        {
            return ValidationResult.Success;
        }

        var text = xhtmlString.ToHtmlString().StripHtml();


        return text.Length <= _max
            ? ValidationResult.Success
            : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}