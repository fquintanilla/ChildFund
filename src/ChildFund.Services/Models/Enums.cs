using System.ComponentModel;

namespace ChildFund.Services.Models
{
    public enum ChildFrequencyEnum
    {
        [Description("Monthly")]
        M,
        [Description("One-time")]
        O,
        [Description("Quarterly")]
        Q,
        [Description("Semi-annually")]
        S,
        [Description("Annually")]
        A
    }

    public enum ChildAcctTypeEnum
    {
        [Description("Sponsorship")]
        SP,
        [Description("Contribution")]
        CO,
        [Description("Designated Fund")]
        DF
    }

    public enum ChildStatusEnum
    {
        [Description("Sponsored")]
        S,
        [Description("Pre-Sponsored")]
        P,
        [Description("Unavailable")]
        U,
        [Description("Reinstatable")]
        R
    }

    public enum ContactTitleSuffix
    {
        Title,
        Suffix
    }

    public enum OrgName
    {
        [Description("CHURCH")]
        CHURCH,
        [Description("CORPORATION")]
        CORPORATION,
        [Description("INC.")]
        INC,
        [Description("SCHOOL")]
        SCHOOL,
        [Description("UNIVERSITY")]
        UNIVERSITY,
        [Description("COLLEGE")]
        COLLEGE,
        [Description("FOUNDATION")]
        FOUNDATION,
        [Description("UNITED WAY")]
        UNITED_WAY
    }

    public enum CreditCardCountries
    {
        US,
        UK,
        CA
    }

    public enum AGPType
    {
        CR,
        DB,
        CH,
        SA
    }

    public enum CardVendor
    {
        MasterCard,
        BankCard,
        Visa,
        AmericanExpress,
        Discover,
        DinersClub,
        EnRoute,
        JCB,
        MC,
        VI,
        AE,
        AMEX,
        DS,
        DI,
        DC,
        CB,
        JC,
        ER
    }

    public enum AgpStatus
    {
        A,
        I
    }

    public enum PaymentType
    {
        ACH,
        CC
    }

    public enum ContactType
    {
        INDV,
        ORG
    }

    public enum SponsorshipContactType
    {
        GF,
        ST
    }

    public enum SponsorshipStatus
    {
        P,
        S,
        N,
        C,
        O,
        R
    }

    public enum EffDateType
    {
        NOW,
        WAIT
    }

    public enum HouseHoldMatch
    {
        HOUSEHOLD,
        ABSOLUTEEXTITLE,
        ABSOLUTE,
        ALL
    }

    public enum Letter
    {
        WEB,
        AGR
    }

    public enum OriginCode
    {
        W
    }

    public enum RelationshipType
    {
        STANDARD,
        GIVER,
        RECIPIENT,
        ORG_CORR,
        ORG_ORIG
    }

    public enum TransactionType
    {
        NEW,
        REPROCESSED,
        PROCESSED,
        EXCEPTION,
        DELETED,
        PENDING,
        LOCKED,
        QUEUED
    }

    public enum ContactMatchType
    {
        NoMatch = 0,
        ContactMatch = 101,
        MultiNameAddressMatch = 102,
        OneHouseHoldMatch = 104,
        MultiHouseHoldMatch = 105
    }

    public enum FinCode
    {
        Donation = 302,
        Sponsorship = 100,
        SponsorshipCambodia = 121,
        SponsorshipVietnam = 119,
        GiftCatalog = 728,
        EssentialsForSurvival = 782,
        Unknown = 0
    }

    public enum PaymentFrequency
    {
        [Description("O")]
        OneTime,

        [Description("M")]
        Monthly,

        [Description("Q")]
        Quarterly,

        [Description("S")]
        SemiAnnually,

        [Description("A")]
        Annually
    }

    public enum DonationTransType
    {
        Sponsorship = 1001,
        GiftCatalog = 1002,
        DesignatedFund = 1003,
        FundAProject = 1004,
        StandardDonation = 1005,
        RecurringDonation = 1006,
        Unknown = 0
    }
}
