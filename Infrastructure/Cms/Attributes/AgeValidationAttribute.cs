using System.ComponentModel.DataAnnotations;

namespace ChildFund.Infrastructure.Cms.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
    public class AgeValidationAttribute : ValidationAttribute
    {
        public int MinimumAgeYears { get; set; }
        public int MaximumAgeYears { get; set; }

        public AgeValidationAttribute(int minimumAgeYears = 10, int maximumAgeYears = 150)
        {
            MinimumAgeYears = minimumAgeYears;
            MaximumAgeYears = maximumAgeYears;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;


            DateTime dob;
            if (MinimumAgeYears == 0) MinimumAgeYears = 10;
            if (MaximumAgeYears == 0) MaximumAgeYears = 150;

            var minAge = Math.Abs(MinimumAgeYears);
            if (minAge == 0) minAge = 10;

            var maxAge = Math.Abs(MaximumAgeYears);
            if (maxAge == 0) maxAge = 150;

            try
            {
                dob = Convert.ToDateTime(value);
            }
            catch (FormatException)
            {
                throw new ValidationException($"{nameof(AgeValidationAttribute)} only works with DateTime values.");
            }
            catch (InvalidCastException)
            {
                throw new ValidationException($"{nameof(AgeValidationAttribute)} only works with DateTime values.");
            }
            catch (Exception)
            {
                throw;
            }

            //At Least 10 Years Old
            if (dob >= DateTime.Now.AddYears(minAge * -1))
            {
                ErrorMessage = $"Age cannot be younger than {minAge} years of age.";
                return false;
            }
            //Not older that 150
            if (dob <= DateTime.Now.AddYears(maxAge * -1))
            {
                ErrorMessage = $"Age cannot be older than {maxAge} years of age.";
                return false;
            }

            return true;
        }
    }
}
