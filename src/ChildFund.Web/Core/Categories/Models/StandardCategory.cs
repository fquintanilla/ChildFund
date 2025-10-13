using Geta.Optimizely.Categories;

namespace ChildFund.Web.Core.Categories.Models;

[ContentType(GUID = "A9BBD7FC-27C5-4718-890A-E28ACBE5EE26",
    DisplayName = "Standard Category",
    Description = "Used to categorize content",
    AvailableInEditMode = false)]
public class StandardCategory : CategoryData
{
    [CultureSpecific]
    [Display(Name = "Title", Description = "If not managed, name will be used instead", Order = 100)]
    public virtual string Title
    {
        get
        {
            var metaTitle = this.GetPropertyValue(p => p.Title);

            return !string.IsNullOrEmpty(metaTitle)
                ? metaTitle
                : Name;
        }
        set => this.SetPropertyValue(p => p.Title, value);
    }
}