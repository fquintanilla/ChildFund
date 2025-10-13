using Geta.Optimizely.Categories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace ChildFund.Web.Infrastructure.Cms.Attributes;

//This is the code from [Categories] attribute. Line 33 is commented just to keep the native content reference list editor
//so we can reorder the categories selected.

[AttributeUsage(AttributeTargets.Property)]
public class GetaCategoriesListAttribute : Attribute, IDisplayMetadataProvider
{
    private readonly CategorySettings _categorySettings;

    private readonly IEnumerable<IContentRepositoryDescriptor> _contentRepositoryDescriptors;

    public GetaCategoriesListAttribute()
        : this(ServiceLocator.Current.GetInstance<IEnumerable<IContentRepositoryDescriptor>>(), ServiceLocator.Current.GetInstance<CategorySettings>())
    {
    }

    public GetaCategoriesListAttribute(IEnumerable<IContentRepositoryDescriptor> contentRepositoryDescriptors, CategorySettings categorySettings)
    {
        _contentRepositoryDescriptors = contentRepositoryDescriptors;
        _categorySettings = categorySettings;
    }

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        if (context.DisplayMetadata.AdditionalValues["epi:extendedmetadata"] is ExtendedMetadata extendedMetadata)
        {
            Type[] value = new Type[1] { typeof(CategoryData) };
            IContentRepositoryDescriptor contentRepositoryDescriptor = _contentRepositoryDescriptors.First((x) => x.Key == CategoryContentRepositoryDescriptor.RepositoryKey);
            //extendedMetadata.ClientEditingClass = "geta-optimizely-categories/widget/CategorySelector";
            extendedMetadata.EditorConfiguration["AllowedTypes"] = value;
            extendedMetadata.EditorConfiguration["AllowedDndTypes"] = value;
            extendedMetadata.OverlayConfiguration["AllowedDndTypes"] = value;
            extendedMetadata.EditorConfiguration["categorySettings"] = _categorySettings;
            extendedMetadata.EditorConfiguration["repositoryKey"] = CategoryContentRepositoryDescriptor.RepositoryKey;
            extendedMetadata.EditorConfiguration["settings"] = contentRepositoryDescriptor;
            extendedMetadata.EditorConfiguration["roots"] = contentRepositoryDescriptor.Roots;
        }
    }
}