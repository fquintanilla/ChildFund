using ChildFund.Web.Features.Shared.Blocks;
using ChildFund.Web.Features.Shared.Interfaces;
using ChildFund.Web.Features.Shared.ViewModels;
using ChildFund.Web.Infrastructure.Cms.Extensions;

namespace ChildFund.Web.Features.Shared.Components;

[TemplateDescriptor(Inherited = true)]
public class DefaultBlockComponent : AsyncBlockComponent<FoundationBlockData>
{
    protected override async Task<IViewComponentResult> InvokeComponentAsync(FoundationBlockData currentContent)
    {
        var model = CreateModel(currentContent);
        return await Task.FromResult(this.View(currentContent, model));
    }

    private static IBlockViewModel<BlockData> CreateModel(BlockData currentBlock)
    {
        var type = typeof(BlockViewModel<>).MakeGenericType(currentBlock.GetOriginalType());
        return Activator.CreateInstance(type, currentBlock) as IBlockViewModel<BlockData>;
    }
}