namespace ChildFund.Features.Shared.Blocks;

public abstract class FoundationBlockData : BlockData
{
    public virtual string BlockName => (this as IContent)?.Name ?? string.Empty;

    public virtual int BlockId => (this as IContent)?.ContentLink?.ID ?? 0;

    public virtual ContentReference ContentLink => (this as IContent)?.ContentLink;
}