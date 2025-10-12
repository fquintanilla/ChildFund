using ChildFund.Features.Shared.Blocks;
using ChildFund.Features.Shared.Interfaces;
using ChildFund.Features.Shared.ViewModels;
using ChildFund.Infrastructure.Cms.Extensions;

namespace ChildFund.Features.Shared.Components;

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