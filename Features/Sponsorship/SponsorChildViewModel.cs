using ChildFund.Core.Models;

namespace ChildFund.Features.Sponsorship
{
    public sealed class SponsorChildViewModel(SponsorChildBlock block, IReadOnlyList<ChildSummaryDto> children, string code)
    {
        public SponsorChildBlock Block { get; } = block;
        public IReadOnlyList<ChildSummaryDto> Children { get; } = children;
        public string Code { get; } = code;
    }
}
