using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Templating;
using Microsoft.AspNetCore.Mvc.Rendering;
using Serilog;

namespace ChildFund.Infrastructure.Rendering;

/// <summary>
/// Allows for a custom content area without extra div tags.
/// </summary>
public class CustomContentAreaRenderer : ContentAreaRenderer
{
    private readonly IContentAreaLoader _contentAreaLoader;
    private readonly IContentRenderer _contentRenderer;
    private readonly IContentAreaItemAttributeAssembler _attributeAssembler;

    public CustomContentAreaRenderer(
        IContentAreaLoader contentAreaLoader,
        IContentRenderer contentRenderer,
        IContentAreaItemAttributeAssembler attributeAssembler)
    {
        _contentAreaLoader = contentAreaLoader;
        _contentRenderer = contentRenderer;
        _attributeAssembler = attributeAssembler;
    }


    protected override void RenderContentAreaItem(
        IHtmlHelper htmlHelper,
        ContentAreaItem contentAreaItem,
        string templateTag,
        string htmlTag,
        string cssClass)
    {
        var renderSettings = new Dictionary<string, object>
        {
            ["childrencustomtagname"] = htmlTag,
            ["childrencssclass"] = cssClass,
            ["tag"] = templateTag
        };

        renderSettings = contentAreaItem.RenderSettings.Concat(
            from r in renderSettings
            where !contentAreaItem.RenderSettings.ContainsKey(r.Key)
            select r).ToDictionary(r => r.Key, r => r.Value);

        htmlHelper.ViewBag.RenderSettings = renderSettings;
        var content = _contentAreaLoader.LoadContent(contentAreaItem);
        if (content == null)
        {
            return;
        }

        bool isInEditMode = IsInEditMode();

        try
        {
            using (new ContentRenderingScope(htmlHelper.ViewContext.HttpContext, content))
            {
                var templateModel = ResolveContentTemplate(htmlHelper, content as IContent, new List<string> { templateTag });
                if (templateModel != null || isInEditMode)
                {
                    var ContentAreaTag = htmlHelper.ViewData["tag"] as string;
                    var tag = string.IsNullOrEmpty(ContentAreaTag) ? templateTag : ContentAreaTag;
                    bool renderWrappingElement = ShouldRenderWrappingElementForContentAreaItem(htmlHelper);
                    bool renderWrappingEditModeElement = ShouldRenderWrappingElementForEditMode(htmlHelper);

                    // The code for this method has been c/p from EPiServer.Web.Mvc.Html.ContentAreaRenderer.
                    // The only difference is this if/else block.
                    if ((isInEditMode || renderWrappingElement) && renderWrappingEditModeElement)
                    {
                        var tagBuilder = new TagBuilder(htmlTag);
                        var unused = AddNonEmptyCssClass(tagBuilder, cssClass);
                        tagBuilder.MergeAttributes(_attributeAssembler.GetAttributes(
                            contentAreaItem,
                            isInEditMode,
                            templateModel != null));
                        BeforeRenderContentAreaItemStartTag(tagBuilder, contentAreaItem);
                        htmlHelper.ViewContext.Writer.Write(tagBuilder.RenderStartTag());
                        htmlHelper.RenderContentData(content, true, templateModel, _contentRenderer);
                        htmlHelper.ViewContext.Writer.Write(tagBuilder.RenderEndTag());
                    }
                    else
                    {
                        // This is the extra code: don't render wrapping elements in view mode
                        htmlHelper.RenderContentData(content, true, templateModel, _contentRenderer);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.Error(e, $"ERROR RENDERING TEMPLATE: {e.Message}");
        }
    }

    private bool ShouldRenderWrappingElementForContentAreaItem(IHtmlHelper htmlHelper)
    {
        // Set 'haschildcontainers' to false by default
        bool? item = (bool?)htmlHelper.ViewContext.ViewData["HasItemContainer"];
        return item == null || !item.HasValue || item.Value;
    }
    private bool ShouldRenderWrappingElementForEditMode(IHtmlHelper htmlHelper)
    {
        bool? item = (bool?)htmlHelper.ViewContext.ViewData["HasEditContainer"];
        return item == null || !item.HasValue || item.Value;
    }
}