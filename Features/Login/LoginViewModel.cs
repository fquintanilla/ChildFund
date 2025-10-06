namespace ChildFund.Features.Login
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public ContentReference ResetPasswordPage { get; set; }

        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberMe { get; set; }
    }
}
