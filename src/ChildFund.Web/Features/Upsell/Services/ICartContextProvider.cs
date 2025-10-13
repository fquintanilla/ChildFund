using ChildFund.Web.Features.Upsell.Models;

namespace ChildFund.Web.Features.Upsell.Services
{
    public interface ICartContextProvider
    {
        CartContext Get();
    }
}
