using ChildFund.Web.Core.Settings;
using ChildFund.Web.Features.Shared.Interfaces;
using ChildFund.Web.Infrastructure.Cms.Settings;

namespace ChildFund.Web.Features.Shared.ViewModels;

public class BlockViewModel<T> : IBlockViewModel<T> where T : BlockData
{
    protected Injected<ISettingsService> _settingsService;
    protected Injected<IPageRouteHelper> _pageRouteHelper;

    public BlockViewModel(T currentBlock)
    {
        CurrentBlock = currentBlock;
        LabelSettings = _settingsService.Service.GetSiteSettings<LabelSettings>();
        PageTypeName = _pageRouteHelper.Service.Page?.PageTypeName ?? string.Empty;
        PageId = _pageRouteHelper.Service.Page?.ContentLink?.ID ?? 0;
    }

    public T CurrentBlock { get; }
    public LabelSettings LabelSettings { get; }
    public string PageTypeName { get; private set; }
    public int PageId { get; private set; }
}