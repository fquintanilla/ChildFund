using ChildFund.Features.Shared.ViewModels;
using ChildFund.Infrastructure.Cms.Extensions;

namespace ChildFund.Features.Home
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