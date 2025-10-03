using EPiServer.Commerce.UI.Admin.Countries.Internal;

namespace ChildFund.Features.MyAccount.AddressBook
{
    public class AddressModel
    {
        public string AddressId { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public IEnumerable<CountryViewModel> CountryOptions { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Line1 { get; set; }

        public string Line2 { get; set; }

        public string Email { get; set; }

        public bool ShippingDefault { get; set; }

        public bool BillingDefault { get; set; }

        public string DaytimePhoneNumber { get; set; }

        public string Organization { get; set; }

        public string ErrorMessage { get; set; }

        public string MultipleAddressLabel => Line1 + ", " + City + ", "  + PostalCode;
    }
}
