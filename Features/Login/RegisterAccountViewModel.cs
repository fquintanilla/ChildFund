using ChildFund.Features.MyAccount.AddressBook;

namespace ChildFund.Features.Login
{
    public class RegisterAccountViewModel
    {
        public AddressModel Address { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Password2 { get; set; }

        public bool Newsletter { get; set; }

        public string ErrorMessage { get; set; }
    }
}
