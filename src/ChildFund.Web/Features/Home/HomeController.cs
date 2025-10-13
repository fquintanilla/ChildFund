using ChildFund.Web.Features.Shared.ViewModels;
using ChildFund.Web.Infrastructure.Cms.Extensions;

namespace ChildFund.Web.Features.Home
{
    public class HomeController : PageController<HomePage>
    {
        public Task<IActionResult> Index(HomePage currentContent)
        {
            var model = ContentViewModel.Create(currentContent);
            return Task.FromResult<IActionResult>(this.View(currentContent, model));
        }
    }
}