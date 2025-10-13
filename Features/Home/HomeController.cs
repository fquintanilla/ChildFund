using ChildFund.Core;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace ChildFund.Features.Home
{
    public class HomeController(IChildFundClient client) : PageController<HomePage>
    {
        public async Task<ActionResult> Index(HomePage currentContent)
        {
            //var child = await client.GetRandomKidsForWebAsync();

            return View("~/Features/Home/Index.cshtml", currentContent);
        } 
    }
}
