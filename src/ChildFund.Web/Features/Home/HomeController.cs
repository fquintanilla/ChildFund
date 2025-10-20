using ChildFund.Web.Features.Shared.ViewModels;
using ChildFund.Web.Infrastructure.Cms.Extensions;
using ChildFund.Web.Repositories;

namespace ChildFund.Web.Features.Home
{
    public class HomeController : PageController<HomePage>
    {
        public async Task<IActionResult> Index(HomePage currentContent)
        {
            var client = ServiceLocator.Current.GetInstance<IDonorServiceRepository>();

            /***These are just examples of how to call the CF API***/

            //var data = await client.FindContacts(new ContactInfoDto(){Email = "cfwebsitetesting@gmail.com"});
            //var data = await client.GetContactById(847166170);
            //var data = await client.GetLTEChildrenByContactId(847166170);

            /*var lookuprepo = ServiceLocator.Current.GetInstance<ILookupServiceRepository>();
            var countries = await lookuprepo.GetAllCountriesAsync();
            var titles = await lookuprepo.GetWebTitlesAsync();*/

            /*var repo = ServiceLocator.Current.GetInstance<IChildServiceRepository>();
            var random = await repo.GetRandomKidsForWeb();
            var withFilter = await repo.GetAvailableKidsForWeb(new Services.Models.ChildFilterDto { Gender = "M" });*/

            var model = ContentViewModel.Create(currentContent);
            return await Task.FromResult<IActionResult>(this.View(currentContent, model));
        }
    }
}