namespace ChildFund.Web.Infrastructure.Commerce
{
    public static class Constant
    {
        public const string ErrorMessages = "ErrorMessages";

        public static class Order
        {
            public const string BudgetPayment = "BudgetPayment";
            public const string PendingApproval = "PendingApproval";
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

        public static class Quote
        {
            public const string QuoteExpireDate = "QuoteExpireDate";
            public const string ParentOrderGroupId = "ParentOrderGroupId";
            public const string QuoteStatus = "QuoteStatus";
            public const string RequestQuotation = "RequestQuotation";
            public const string RequestQuotationFinished = "RequestQuotationFinished";
            public const string PreQuoteTotal = "PreQuoteTotal";
            public const string PreQuotePrice = "PreQuotePrice";
            public const string QuoteExpired = "QuoteExpired";
            public const string RequestQuoteStatus = "RequestQuoteStatus";
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
