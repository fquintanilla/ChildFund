using System.Collections;

namespace ChildFund.Web.Infrastructure.Cms.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class MaxElementsAttribute : ValidationAttribute
{
    private readonly int _maxItemAllowed;

    public MaxElementsAttribute(int maxItemAllowed) => _maxItemAllowed = maxItemAllowed;

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return null;
        }

        if (value is LinkItemCollection linkItemCollection && linkItemCollection.Count > _maxItemAllowed)
        {
            return new ValidationResult($"Link Item Collection exceeds the maximum limit of {_maxItemAllowed} item(s)");
        }

        if (value is IList<string> list && list.Count > _maxItemAllowed)
        {
            return new ValidationResult(
                $"{validationContext.DisplayName} exceeds the maximum limit of {_maxItemAllowed} item(s)");
        }

        if (value is IList<ContentReference> contentList && contentList.Count > _maxItemAllowed)
        {
            return new ValidationResult(
                $"{validationContext.DisplayName} exceeds the maximum limit of {_maxItemAllowed} item(s)");
        }

        if (value is ContentArea contentArea && contentArea.Count > _maxItemAllowed)
        {
            var visitorGroupCounts = new Dictionary<string, int>();
            const string everyone = "Everyone";
            foreach (var item in contentArea.Items)
            {
                if (item.AllowedRoles == null || !item.AllowedRoles.Any())
                {
                    if (!visitorGroupCounts.ContainsKey(everyone))
                    {
                        visitorGroupCounts.Add(everyone, 1);
                    }
                    else
                    {
                        var last = visitorGroupCounts[everyone];
                        visitorGroupCounts[everyone] = last + 1;
                    }

                    continue;
                }

                foreach (var role in item.AllowedRoles)
                {
                    if (!visitorGroupCounts.ContainsKey(role))
                    {
                        visitorGroupCounts.Add(role, 1);
                    }
                    else
                    {
                        var last = visitorGroupCounts[role];
                        visitorGroupCounts[role] = last + 1;
                    }
                }
            }

            var everyoneCount = 0;
            if (visitorGroupCounts.Count > 0 && visitorGroupCounts.Keys.Contains(everyone))
            {
                everyoneCount = visitorGroupCounts[everyone];
            }

            if (everyoneCount > _maxItemAllowed)
            {
                ErrorMessage = $"Content area exceeds the maximum limit of {_maxItemAllowed} content items";
                return new ValidationResult(ErrorMessage);
            }

            foreach (var key in visitorGroupCounts.Keys.Where(key => key != everyone).Where(key => visitorGroupCounts[key] > _maxItemAllowed))
            {
                ErrorMessage = $"Content area exceeds the maximum limit of {_maxItemAllowed} content items for {key} visitor group.";
                return new ValidationResult(ErrorMessage);
            }

            return null;
        }

        if (value is IList genericList && genericList.Count > _maxItemAllowed)
        {
            return new ValidationResult(
                $"{validationContext.DisplayName} exceeds the maximum limit of {_maxItemAllowed} item(s)");
        }

        return null;
    }
}