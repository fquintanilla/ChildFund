using ChildFund.Web.Features.Shared.ViewModels;

namespace ChildFund.Web.Features.MyAccount.AddressBook
{
    public class AddressCollectionViewModel : ContentViewModel<AddressBookPage>
    {
        public AddressCollectionViewModel()
        {
        }

        public AddressCollectionViewModel(AddressBookPage currentPage) : base(currentPage) { }

        public IEnumerable<AddressModel> Addresses { get; set; }
    }
}
