using ChildFund.Features.Login;
using ChildFund.Features.MyAccount.AddressBook;
using ChildFund.Features.Shared;
using ChildFund.Features.Shared.ViewModels;

namespace ChildFund.Features.Checkout
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
