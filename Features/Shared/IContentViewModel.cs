using ChildFund.Features.Home;
using Microsoft.AspNetCore.Html;

namespace ChildFund.Features.Shared
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        HomePage StartPage { get; }
        HtmlString SchemaMarkup { get; }
    }
}
