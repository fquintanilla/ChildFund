using ChildFund.Features.Upsell.Models;

namespace ChildFund.Features.Upsell.Services
{
    public interface ICartContextProvider
    {
        CartContext Get();
    }
}
