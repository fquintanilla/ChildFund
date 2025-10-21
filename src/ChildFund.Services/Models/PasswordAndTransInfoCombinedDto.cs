namespace ChildFund.Services.Models;

/// <summary>
/// Combined password reset and transaction information DTO - matches WCF auto-generated model.
/// </summary>
public partial class PasswordAndTransInfoCombinedDto
{
    public PasswordResetInfoDto? Info { get; set; }
    public TransactionInfoDto? TransInfo { get; set; }
}
