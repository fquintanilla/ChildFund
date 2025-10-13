using ChildFund.Web.Core.Settings;

namespace ChildFund.Web.Features.Shared.Interfaces;

public interface IBlockViewModel<out T> where T : BlockData
{
    T CurrentBlock { get; }
    LabelSettings LabelSettings { get; }
    string PageTypeName { get; }
    int PageId { get; }
}