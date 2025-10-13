using ChildFund.Web.Core.Media.Models;
using ChildFund.Web.Core.Media.ViewModels;
using ChildFund.Web.Infrastructure.Cms.Extensions;
using ChildFund.Web.Infrastructure.Cms.Helpers;

namespace ChildFund.Web.Core.Media.Controllers;

[TemplateDescriptor(TemplateTypeCategory = TemplateTypeCategories.MvcPartialComponent, Inherited = true)]
public class MediaController : PartialContentComponent<MediaData>
{
    private readonly IContextModeResolver _contextModeResolver;
    private readonly UrlResolver _urlResolver;

    public MediaController(UrlResolver urlResolver, IContextModeResolver contextModeResolver)
    {
        _urlResolver = urlResolver;
        _contextModeResolver = contextModeResolver;
    }

    protected override IViewComponentResult InvokeComponent(MediaData currentContent)
    {
        switch (currentContent)
        {
            case VideoFile videoFile:
                var videoViewModel = GenerateVideoFileViewModel(videoFile);
                return View("~/Core/Media/Views/VideoFile.cshtml", videoViewModel);
            case VectorImageMediaData svg:
                var svgViewModel = GenerateSvgMediaDataViewModel(svg);
                return View("~/Core/Media/Views/SvgMedia.cshtml", svgViewModel);
            case ImageMediaData image:
                var imageViewModel = GenerateImageMediaDataViewModel(image);
                return View("~/Core/Media/Views/ImageMedia.cshtml", imageViewModel);
            case DocumentMediaData document:
                var documentViewModel = GenerateDocumentViewModel(document);
                return View("~/Core/Media/Views/Document.cshtml", documentViewModel);
            default:
                return View("~/Core/Media/Views/Index.cshtml", currentContent.GetType().BaseType.Name);
        }
    }

    private VideoFileViewModel GenerateVideoFileViewModel(VideoFile videoFile)
    {
        var videoViewModel = new VideoFileViewModel
        {
            DisplayControls = videoFile.DisplayControls,
            Autoplay = videoFile.Autoplay,
            Copyright = videoFile.Copyright
        };

        if (_contextModeResolver.CurrentMode == ContextMode.Edit)
        {
            videoViewModel.VideoLink = _urlResolver.GetUrl(videoFile.ContentLink, null,
                new VirtualPathArguments { ContextMode = ContextMode.Default });
        }
        else
        {
            videoViewModel.VideoLink = _urlResolver.GetUrl(videoFile.ContentLink);
        }

        return videoViewModel;
    }

    private ImageMediaDataViewModel GenerateImageMediaDataViewModel(ImageMediaData image)
    {
        var imageViewModel = new ImageMediaDataViewModel
        {
            Name = image.Name,
            AltText = image.AltText,
            Description = image.Description,
            DefaultPictureSizes = ImageHelper.CreateDefaultPictureSizeList(image)
        };

        if (_contextModeResolver.CurrentMode == ContextMode.Edit)
        {
            imageViewModel.ImageLink = _urlResolver.GetUrl(image.ContentLink, null,
                new VirtualPathArguments { ContextMode = ContextMode.Default });
        }
        else
        {
            imageViewModel.ImageLink = _urlResolver.GetUrl(image.ContentLink);
        }

        return imageViewModel;
    }

    private DocumentViewModel GenerateDocumentViewModel(DocumentMediaData document)
    {
        var model = new DocumentViewModel { Name = document.Name, Description = document.Description };

        if (_contextModeResolver.CurrentMode == ContextMode.Edit)
        {
            model.Link = _urlResolver.GetUrl(document.ContentLink, null,
                new VirtualPathArguments { ContextMode = ContextMode.Default });
        }
        else
        {
            model.Link = _urlResolver.GetUrl(document.ContentLink);
        }

        return model;
    }

    private SvgMediaDataViewModel GenerateSvgMediaDataViewModel(VectorImageMediaData svg) =>
        new SvgMediaDataViewModel
        {
            Name = svg.Name,
            Description = svg.Description,
            Data = svg.GetSvgImage(),
            UseHtmlTag = svg.UseHtmlTag,
            ImageLink = Url.ContentUrl(svg.ContentLink)
        };
}