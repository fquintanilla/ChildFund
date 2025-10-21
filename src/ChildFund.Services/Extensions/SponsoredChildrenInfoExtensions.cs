using ChildFund.Services.Models;

namespace ChildFund.Services.Extensions
{
    public static class SponsoredChildrenInfoExtension
    {
        public static ChildFrequencyEnum FreqEnum(this SponsoredChildrenInfoDto sponsoredChildrenInfo)
        {
            return WebEnumHelper<ChildFrequencyEnum>.Parse(sponsoredChildrenInfo.Frequency);
        }
        public static string FreqDesc(this SponsoredChildrenInfoDto sponsoredChildrenInfo)
        {
            return WebEnumHelper<ChildFrequencyEnum>.GetDescription(sponsoredChildrenInfo.Frequency);
        }
        public static ChildAcctTypeEnum AcctTypeEnum(this SponsoredChildrenInfoDto sponsoredChildrenInfo)
        {
            return WebEnumHelper<ChildAcctTypeEnum>.Parse(sponsoredChildrenInfo.AcctType);
        }
        public static string AcctTypeDesc(this SponsoredChildrenInfoDto sponsoredChildrenInfo)
        {
            return WebEnumHelper<ChildAcctTypeEnum>.GetDescription(sponsoredChildrenInfo.AcctType);
        }
        public static ChildStatusEnum StatusEnum(this SponsoredChildrenInfoDto sponsoredChildrenInfo)
        {
            return WebEnumHelper<ChildStatusEnum>.Parse(sponsoredChildrenInfo.Status);
        }
        public static string StatusDesc(this SponsoredChildrenInfoDto sponsoredChildrenInfo)
        {
            return WebEnumHelper<ChildStatusEnum>.GetDescription(sponsoredChildrenInfo.Status);
        }
    }
}
