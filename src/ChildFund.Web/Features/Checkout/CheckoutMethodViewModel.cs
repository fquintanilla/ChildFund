using ChildFund.Web.Features.Login;
using ChildFund.Web.Features.MyAccount.AddressBook;
using ChildFund.Web.Features.Shared.ViewModels;

namespace ChildFund.Web.Features.Checkout
{
    public class CheckoutMethodViewModel : ContentViewModel<CheckoutPage>
    {
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }

        public CheckoutMethodViewModel() : base()
        {
            LoginViewModel = new LoginViewModel();
            RegisterAccountViewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };
        }

        public CheckoutMethodViewModel(CheckoutPage currentPage, string returnUrl = "/") : base(currentPage)
        {
            LoginViewModel = new LoginViewModel();
            RegisterAccountViewModel = new RegisterAccountViewModel
            {
                Address = new AddressModel()
            };
            LoginViewModel.ReturnUrl = returnUrl;
            CurrentContent = currentPage;
        }
    }
}
