namespace ChildFund.Web.Infrastructure.Cms.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class RequiredGreaterThanZero : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            // return true if value is a non-null number > 0, otherwise return false
            return value != null && int.TryParse(value.ToString(), out var number) && number > 0;
        }
    }
}
