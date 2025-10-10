namespace ChildFund.Core.Options
{
    public sealed class ChildFundOptions
    {
        /// <summary>Base URL, e.g. "https://pubwebapi.childfund.org/api/v1"</summary>
        public string BaseUrl { get; set; } = null!;

        /// <summary>Pre-encoded "User=..." payload expected by Authenticate endpoint.</summary>
        public string ApiKey { get; set; } = null!;

        /// <summary>Relative path to authenticate, default "Account/Authenticate"</summary>
        public string AuthenticatePath { get; set; } = "Account/Authenticate/";
    }
}
