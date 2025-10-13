using EPiServer.SpecializedProperties;
using System.ComponentModel.DataAnnotations;

namespace ChildFund.Infrastructure.Cms.Attributes
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredForPublishAttribute : ValidationAttribute
    {
        public bool IsRequired { get; set; }

        public RequiredForPublishAttribute() : this(true) { }

        public RequiredForPublishAttribute(bool isRequired)
        {
            IsRequired = isRequired;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;
            if (value is string str) return !string.IsNullOrEmpty(str);
            if (value is ContentReference reference) return reference != null && reference != ContentReference.EmptyReference;
            if (value is ContentArea area) return area?.Items?.Any() == true;
            if (value is IEnumerable<object> enumer) return enumer?.Any() == true;
            if (value is XhtmlString html) return !string.IsNullOrEmpty(html.ToHtmlString());
            if (value is LinkItemCollection links) return links?.Any() == true;
            if (value is Url link) return link?.IsEmpty() != true;
            if (value is LinkItem linkItem) return !string.IsNullOrEmpty(linkItem.Href);
            if (value is DateTime dateTime) return dateTime != DateTime.MinValue;
            if (value is int integer) return integer > 0;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Property '{name}' is required.";
        }
    }
}
