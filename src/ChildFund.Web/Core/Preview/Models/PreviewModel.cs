using ChildFund.Web.Features.Shared.Pages;
using ChildFund.Web.Features.Shared.ViewModels;

namespace ChildFund.Web.Core.Preview.Models;

public class PreviewModel : ContentViewModel<FoundationPageData>
{
    public PreviewModel(FoundationPageData currentPage, IContent previewContent) : base(currentPage)
    {
        PreviewContent = previewContent;
        Areas = new List<PreviewArea>();
    }

    public IContent PreviewContent { get; set; }
    public List<PreviewArea> Areas { get; set; }

    public class PreviewArea
    {
        public bool Supported { get; set; }
        public string AreaName { get; set; }
        public string AreaTag { get; set; }
        public ContentArea ContentArea { get; set; }
    }
}