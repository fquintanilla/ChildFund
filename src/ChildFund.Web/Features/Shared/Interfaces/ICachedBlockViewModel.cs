namespace ChildFund.Web.Features.Shared.Interfaces;

public interface ICachedBlockViewModel<out T> : IBlockViewModel<T> where T : BlockData
{
    public string Version { get; set; }
}