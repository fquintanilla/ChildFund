using ChildFund.Services;
using ChildFund.Services.Interfaces;
using ChildFund.Web.Features.Shared.ViewModels;
using ChildFund.Web.Infrastructure.Cms.Extensions;

namespace ChildFund.Web.Features.Home
{
    public class HomeController : PageController<HomePage>
    {
        public async Task<IActionResult> Index(HomePage currentContent)
        {
            var client = ServiceLocator.Current.GetInstance<IDonorPortalClient>();
            //var data = await client.FindContactsAsync("cfwebsitetesting@gmail.com");
            //var data = await client.GetContactByIdAsync("847166170");
            //var data = await client.GetLteChildrenByContactId("847166170");


            var model = ContentViewModel.Create(currentContent);
            return await Task.FromResult<IActionResult>(this.View(currentContent, model));
        }
    }
}