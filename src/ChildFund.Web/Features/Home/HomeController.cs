using ChildFund.Services.Interfaces;
using ChildFund.Web.Features.Shared.ViewModels;
using ChildFund.Web.Infrastructure.Cms.Extensions;
using ChildFund.Web.Repositories;

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

            /*var repo = ServiceLocator.Current.GetInstance<ILookupServiceRepository>();
            var countries = await repo.GetAllCountriesAsync();
            var titles = await repo.GetWebTitlesAsync();*/

            var repo = ServiceLocator.Current.GetInstance<IChildServiceRepository>();
            var random = await repo.GetRandomKidsForWeb();
            var withFilter = await repo.GetAvailableKidsForWeb(new Services.Models.ChildFilterDto { Gender = "M" });

            var model = ContentViewModel.Create(currentContent);
            return await Task.FromResult<IActionResult>(this.View(currentContent, model));
        }
    }
}