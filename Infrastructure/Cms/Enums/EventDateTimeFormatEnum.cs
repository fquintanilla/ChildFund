using System.ComponentModel;

namespace ChildFund.Infrastructure.Cms.Enums;

public enum EventDateTimeFormatEnum
{
    [Description("MMM dd, yyyy")]
    MastheadDate = 0,

    [Description("h:mm tt")]
    MastheadTime = 1,

    [Description("MMM")]
    CardMonths = 2,

    [Description("dd")]
    CardDays = 3
}
