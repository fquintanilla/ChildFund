using ChildFund.Web.Core.Settings;
using ChildFund.Web.Features.Home;

namespace ChildFund.Web.Features.Shared.Interfaces;

public interface IContentViewModel<out TContent> where TContent : IContent
{
    TContent CurrentContent { get; }
    bool HideHeader { get; set; }
    bool HideFooter { get; set; }
    bool TrackOdp { get; set; }
    HomePage StartPage { get; }
    LayoutSettings Layout { get; set; }
    LabelSettings Labels { get; set; }
    SiteSettings SiteSettings { get; set; }
    ReferencePageSettings PageReferences { get; set; }
    IHtmlContent SchemaMarkup { get; }
}