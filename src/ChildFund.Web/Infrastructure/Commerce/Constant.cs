namespace ChildFund.Web.Infrastructure.Commerce
{
    public static class Constant
    {
        public const string ErrorMessages = "ErrorMessages";

        public static class Order
        {
            public const string BudgetPayment = "BudgetPayment";
        }

        public static class Fields
        {
            public const string UserRole = "UserRole";
        }

        public static class Customer
        {
            public const string CustomerFullName = "CustomerFullName";
            public const string CustomerEmailAddress = "CustomerEmailAddress";
            public const string CurrentCustomerOrganization = "CurrentCustomerOrganization";
        }
        
        public static class LineItemFields
        {
            public const string ChildId = "ChildId";
            public const string ChildName = "ChildName";
            public const string PaymentFrequency = "PaymentFrequency";
            public const string IsCustomPrice = "IsCustomPrice";
        }
    }
}
