using ChildFund.Services.Models;

namespace ChildFund.Web.Features.Sponsorship
{
    public sealed class SponsorChildViewModel(SponsorChildBlock block, IReadOnlyList<WebChildInfoDto> children, string code)
    {
        public SponsorChildBlock Block { get; } = block;
        public IReadOnlyList<WebChildInfoDto> Children { get; } = children;
        public string Code { get; } = code;
    }
}
