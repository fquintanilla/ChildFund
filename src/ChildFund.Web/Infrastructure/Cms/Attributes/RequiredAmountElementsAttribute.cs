namespace ChildFund.Web.Infrastructure.Cms.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAmountElementsAttribute : ValidationAttribute
    {
        private readonly int _requiredAmountOfItems;

        public RequiredAmountElementsAttribute(int maxItemAllowed) => _requiredAmountOfItems = maxItemAllowed;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                ErrorMessage = $"{validationContext.DisplayName} requires exactly {_requiredAmountOfItems} item(s)";
                return new ValidationResult(ErrorMessage);
            }

            if (value is LinkItemCollection linkItemCollection && linkItemCollection.Count != _requiredAmountOfItems)
            {
                return new ValidationResult($"Link Item Collection requires exactly {_requiredAmountOfItems} item(s)");
            }

            if (value is IList<string> list && list.Count != _requiredAmountOfItems)
            {
                return new ValidationResult($"{validationContext.DisplayName} requires exactly {_requiredAmountOfItems} item(s)");
            }

            if (value is IList<ContentReference> contentList && contentList.Count != _requiredAmountOfItems)
            {
                return new ValidationResult($"{validationContext.DisplayName} requires exactly {_requiredAmountOfItems} item(s)");
            }

            if (value is ContentArea contentArea && contentArea.Count != _requiredAmountOfItems)
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

                if (everyoneCount != _requiredAmountOfItems)
                {
                    ErrorMessage = $"Content area restricted to exactly {_requiredAmountOfItems} content items";
                    return new ValidationResult(ErrorMessage);
                }

                foreach (var key in visitorGroupCounts.Keys.Where(key => key != everyone).Where(key => visitorGroupCounts[key] != _requiredAmountOfItems))
                {
                    ErrorMessage = $"Content area restricted to exactly {_requiredAmountOfItems} content items for {key} visitor group.";
                    return new ValidationResult(ErrorMessage);
                }

                return null;
            }

            return null;
        }
    }
}
