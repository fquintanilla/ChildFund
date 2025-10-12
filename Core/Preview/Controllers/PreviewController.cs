using ChildFund.Core.Preview.Models;
using ChildFund.Features.Home;

namespace ChildFund.Core.Preview.Controllers;

[TemplateDescriptor(
    Inherited = true,
    TemplateTypeCategory =
        TemplateTypeCategories
            .MvcController, //Required as controllers for blocks are registered as MvcPartialController by default
    Tags = new[] { RenderingTags.Preview, RenderingTags.Edit },
    AvailableWithoutTag = false)]
public class PreviewController : ActionControllerBase, IRenderTemplate<BlockData>
{
    private readonly IContentLoader _contentLoader;
    private readonly DisplayOptions _displayOptions;
    private readonly TemplateResolver _templateResolver;

    public PreviewController(IContentLoader contentLoader, TemplateResolver templateResolver,
        DisplayOptions displayOptions)
    {
        _contentLoader = contentLoader;
        _templateResolver = templateResolver;
        _displayOptions = displayOptions;
    }

    public ActionResult RenderResult(IContent currentContent)
    {
        //As the layout requires a page for title etc we "borrow" the start page
        var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);

        var model = new PreviewModel(startPage, currentContent);

        var supportedDisplayOptions = _displayOptions
            .Select(x => new { x.Tag, x.Name, Supported = SupportsTag(currentContent, x.Tag) }).ToList();

        if (!supportedDisplayOptions.Any(x => x.Supported))
        {
            return new ViewResult
            {
                ViewName = "~/Core/Preview/Views/Index.cshtml",
                ViewData = new ViewDataDictionary<PreviewModel>(ViewData, model)
            };
        }

        foreach (var displayOption in supportedDisplayOptions)
        {
            var contentArea = new ContentArea();
            contentArea.Items.Add(new ContentAreaItem { ContentLink = currentContent.ContentLink });
            var areaModel = new PreviewModel.PreviewArea
            {
                Supported = displayOption.Supported,
                AreaTag = displayOption.Tag,
                AreaName = displayOption.Name,
                ContentArea = contentArea
            };

            model.Areas.Add(areaModel);
        }

        return new ViewResult
        {
            ViewName = "~/Core/Preview/Views/Index.cshtml",
            ViewData = new ViewDataDictionary<PreviewModel>(ViewData, model)
        };
    }

    private bool SupportsTag(IContent content, string tag)
    {
        var templateModel = _templateResolver.Resolve(HttpContext,
            content.GetOriginalType(),
            content,
            TemplateTypeCategories.MvcPartial,
            tag);

        return templateModel != null;
    }
}