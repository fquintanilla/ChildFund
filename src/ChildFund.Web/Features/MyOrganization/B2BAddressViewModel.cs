using ChildFund.Web.Features.MyAccount.AddressBook;
using ChildFund.Web.Infrastructure.Commerce.Customer;

namespace ChildFund.Web.Features.MyOrganization
{
    public class B2BAddressViewModel
    {
        public B2BAddressViewModel(FoundationAddress address)
        {
            AddressId = address.AddressId;
            Name = address.Name;
            Street = address.Street;
            City = address.City;
            PostalCode = address.PostalCode;
            CountryCode = address.CountryCode;
            CountryName = address.CountryName;
        }

        public B2BAddressViewModel()
        {
        }

        public Guid AddressId { get; set; }

        public string Name { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string CountryCode { get; set; }

        public string CountryName { get; set; }

        public IEnumerable<CountryViewModel> CountryOptions { get; set; }

        public string AddressString => Street + " " + City + " " + PostalCode + " " + CountryName;
    }
}
