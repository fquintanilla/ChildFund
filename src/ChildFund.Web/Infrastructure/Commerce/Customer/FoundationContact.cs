using ChildFund.Web.Infrastructure.Commerce.Extensions;
using Mediachase.BusinessFoundation.Data;
using Mediachase.Commerce.Customers;

namespace ChildFund.Web.Infrastructure.Commerce.Customer
{
    public class FoundationContact
    {
        public FoundationContact() => Contact = new CustomerContact();

        public FoundationContact(CustomerContact contact) => Contact = contact ?? new CustomerContact();

        public CustomerContact Contact { get; }

        public Guid ContactId
        {
            get => Contact?.PrimaryKeyId ?? Guid.Empty;
            set => Contact.PrimaryKeyId = new PrimaryKeyId(value);
        }

        public string FirstName
        {
            get => Contact.FirstName;
            set => Contact.FirstName = value;
        }

        public IEnumerable<CreditCard> CreditCards
        {
            get => Contact.ContactCreditCards;
        }
        public string LastName
        {
            get => Contact.LastName;
            set => Contact.LastName = value;
        }

        public string FullName
        {
            get => Contact.FullName;
            set => Contact.FullName = value;
        }

        public DateTime? BirthDate
        {
            get => Contact.BirthDate;
            set => Contact.BirthDate = value;
        }

        public string Email
        {
            get => Contact.Email;
            set => Contact.Email = value;
        }

        public string UserRole
        {
            get => Contact.GetStringValue(Constant.Fields.UserRole);
            set => Contact[Constant.Fields.UserRole] = value;
        }


        public B2BUserRoles B2BUserRole
        {
            get
            {
                var parsed = Enum.TryParse(UserRole, out B2BUserRoles retVal);
                return parsed ? retVal : B2BUserRoles.None;
            }
        }

        public FoundationOrganization FoundationOrganization
        {
            get => Contact != null && Contact.ContactOrganization != null ? new FoundationOrganization(Contact.ContactOrganization) : null;
            set => Contact.OwnerId = value.OrganizationEntity.PrimaryKeyId;
        }

        // The UserId needs to be set in the format "String:{email}". Else a duplicate CustomerContact will be created later on.
        public string UserId
        {
            get => Contact.UserId;
            set => Contact.UserId = $"String:{value}";
        }

        public string Bookmarks
        {
            get => Contact.GetStringValue("Bookmarks");
            set => Contact["Bookmarks"] = value;
        }

        public string RegistrationSource
        {
            get => Contact.RegistrationSource;
            set => Contact.RegistrationSource = value;
        }

        public bool AcceptMarketingEmail
        {
            get => Contact.AcceptMarketingEmail;
            set => Contact.AcceptMarketingEmail = value;
        }

        public DateTime? ConsentUpdated
        {
            get => Contact.ConsentUpdated;
            set => Contact.ConsentUpdated = value;
        }

        public string DemoUserTitle
        {
            get => Contact.GetStringValue("DemoUserTitle");
            set => Contact["DemoUserTitle"] = value;
        }

        public string DemoUserDescription
        {
            get => Contact.GetStringValue("DemoUserDescription");
            set => Contact["DemoUserDescription"] = value;
        }

        public int ShowInDemoUserMenu
        {
            get => Contact.GetIntegerValue("ShowInDemoUserMenu");
            set => Contact["ShowInDemoUserMenu"] = value;
        }

        public int DemoSortOrder
        {
            get => Contact.GetIntegerValue("DemoSortOrder");
            set => Contact["DemoSortOrder"] = value;
        }

        public bool IsAdmin => B2BUserRole == B2BUserRoles.Admin;

        public string ElevatedRole
        {
            get => Contact.GetStringValue("ElevatedRole");
            set => Contact["ElevatedRole"] = value;
        }

        public bool ShowOrganizationError { get; set; }

        public void SaveChanges() => Contact.SaveChanges();

        public static FoundationContact New() => new FoundationContact(CustomerContact.CreateInstance());
    }
}
