using ChildFund.Features.Shared.Pages;
using EPiServer.Validation;

namespace ChildFund.Infrastructure.Validators;

public class SampleValidator : IValidate<FoundationPageData>
{
    public IEnumerable<ValidationError> Validate(FoundationPageData instance)
    {
        if (instance == null)
        {
            yield return new ValidationError
            {
                ErrorMessage = "Error!",
                Severity = ValidationErrorSeverity.Error,
                PropertyName = "PROPNAME",
                RelatedProperties = new[] { "RELATEDPROPS" }
            };
        }
    }
}