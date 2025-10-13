namespace ChildFund.Web.Features.MyAccount.AddressBook
{
    public class AddressModel
    {
        public string? AddressId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? CountryName { get; set; }

        [Required]
        public string CountryCode { get; set; }

        public IEnumerable<CountryViewModel>? CountryOptions { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Line1 { get; set; }

        public string? Line2 { get; set; }

        public string? Email { get; set; }

        public bool ShippingDefault { get; set; }

        public bool BillingDefault { get; set; }

        public string? DaytimePhoneNumber { get; set; }

        public string? Organization { get; set; }

        public string? ErrorMessage { get; set; }

        public string MultipleAddressLabel => Line1 + ", " + City + ", " + PostalCode;
    }
}
