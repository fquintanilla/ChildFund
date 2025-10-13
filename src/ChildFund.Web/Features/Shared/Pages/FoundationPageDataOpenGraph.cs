using Boxed.AspNetCore.TagHelpers.OpenGraph;
using ChildFund.Web.Infrastructure.Cms.Extensions;

namespace ChildFund.Web.Features.Shared.Pages;

public class FoundationPageDataOpenGraph : OpenGraphMetadata
{
    public FoundationPageDataOpenGraph(string title, OpenGraphImage image, string url = null)
    {
        Title = title;
        if (image != null)
        {
            MainImage = image;
        }

        if (!string.IsNullOrEmpty(url))
        {
            Url = new Uri(url);
        }
    }

    public override string Namespace => "website: http://ogp.me/ns/article#";

    public override OpenGraphType Type => OpenGraphType.Article;

    public IEnumerable<string> Category { get; set; }
    public string Author { get; set; }
    public DateTime? PublishedTime { get; set; }
    public DateTime ModifiedTime { get; set; }
    public DateTime? ExpirationTime { get; set; }

    public override void ToString(StringBuilder stringBuilder)
    {
        base.ToString(stringBuilder);
        stringBuilder.AppendMetaPropertyContentIfNotNull("article:author", Author);
    }
}