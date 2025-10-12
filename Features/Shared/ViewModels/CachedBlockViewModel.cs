using ChildFund.Core.Settings;
using ChildFund.Features.Shared.Interfaces;
using ChildFund.Infrastructure.Cms.Settings;

namespace ChildFund.Features.Shared.ViewModels;

public class CachedBlockViewModel<T> : ICachedBlockViewModel<T> where T : BlockData
{
    protected Injected<ISettingsService> _settingsService;
    protected Injected<IPageRouteHelper> _pageRouteHelper;

    public CachedBlockViewModel(T currentBlock)
    {
        CurrentBlock = currentBlock;
        LabelSettings = _settingsService.Service.GetSiteSettings<LabelSettings>();
        PageTypeName = _pageRouteHelper.Service.Page?.PageTypeName ?? string.Empty;
        PageId = _pageRouteHelper.Service.Page?.ContentLink?.ID ?? 0;
    }

    public string Version { get; set; }
    public T CurrentBlock { get; }
    public LabelSettings LabelSettings { get; }
    public string PageTypeName { get; private set; }
    public int PageId { get; private set; }
}