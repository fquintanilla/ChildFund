using ChildFund.Features.Shared.Interfaces;
using ChildFund.Features.Shared.ViewModels;
using ChildFund.Infrastructure.Rendering;

namespace ChildFund.Infrastructure.Cms.Extensions;

public static class ContentAreaItemExtensions
{
    private static readonly Lazy<IContentLoader> _contentLoader =
        new(() => ServiceLocator.Current.GetInstance<IContentLoader>());

    private static readonly Lazy<CustomContentAreaRenderer> _customContentAreaRenderer =
        new(() => ServiceLocator.Current.GetInstance<CustomContentAreaRenderer>());

    public static IList<T> GetContentItems<T>(this IEnumerable<ContentAreaItem> contentAreaItems) where T : IContentData
    {
        var areaItems = contentAreaItems.ToList();
        if (!areaItems.Any())
        {
            return null;
        }

        if (areaItems?.Any(_ => _.InlineBlock == null) == true)
        {
            return _contentLoader.Value
                .GetItems(areaItems.Select(_ => _.ContentLink),
                    new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() })
                .OfType<T>()
                .ToList();
        }

        var loadedItems = new List<T>();

        foreach (var a in areaItems)
        {
            if (a.InlineBlock is T block)
            {
                loadedItems.Add(block);
            }

            if(a.ContentLink != null && a.ContentLink is T)
            {
                loadedItems.Add(_contentLoader.Value.Get<T>(a.ContentLink,
                    new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() }));
            }
        }

        return loadedItems;

    }

    public static IList<T> GetContentItems<T>(this IEnumerable<ContentAreaItem> contentAreaItems, string language) where T : IContentData
    {
        var areaItems = contentAreaItems?.ToList();
        if (contentAreaItems == null || !areaItems.Any())
        {
            return new List<T>();
        }

        return !string.IsNullOrEmpty(language) ? _contentLoader.Value
                .GetItems(areaItems.Select(_ => _.ContentLink), new LoaderOptions { LanguageLoaderOption.Specific(new CultureInfo(language)) })
                .OfType<T>()
                .ToList() :
            _contentLoader.Value
                .GetItems(areaItems.Select(_ => _.ContentLink), new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() })
                .OfType<T>()
                .ToList();
    }

    public static T GetBlock<T>(this ContentAreaItem contentAreaItem) where T : BlockData
    {
        if (contentAreaItem == null)
        {
            return null;
        }

        return _contentLoader.Value.Get<IContentData>(contentAreaItem.ContentLink) as T;
    }

    public static IBlockViewModel<T> GetBlockViewModel<T>(this ContentAreaItem contentAreaItem) where T : BlockData
    {
        var block = GetBlock<T>(contentAreaItem);
        return block != null ? CreateModel(block) : null;
    }

    private static IBlockViewModel<T> CreateModel<T>(T currentBlock) where T : BlockData
    {
        var type = typeof(BlockViewModel<>).MakeGenericType(currentBlock.GetOriginalType());
        return Activator.CreateInstance(type, currentBlock) as IBlockViewModel<T>;
    }

    public static void RenderCustomContentArea(this HtmlHelper htmlHelper, ContentArea contentArea)
    {
        _customContentAreaRenderer.Value.Render(htmlHelper, contentArea);
    }

    public static bool AreAllObjectsOfSameType(this IEnumerable<ContentAreaItem> contentAreaItems)
    {
        var contentDataItems = contentAreaItems?.GetContentItems<ContentData>();

        if(contentDataItems == null) return true;

        var firstType = contentDataItems.FirstOrDefault()?.GetType();

        return contentDataItems.All(contentDataItem => contentDataItem.GetType() == firstType);
    }
}