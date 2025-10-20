using System.Text.Json.Serialization;

namespace ChildFund.Services.Models;

/// <summary>
/// Contact update information DTO - matches WCF auto-generated model with k__BackingField naming.
/// </summary>
public partial class ContactUpdateInfoDto
{
    [JsonPropertyName("_datemodified")]
    public DateTime? DateModified { get; set; }

    [JsonPropertyName("<HashedAnswer>k__BackingField")]
    public string? HashedAnswer { get; set; }

    [JsonPropertyName("<QuestionId>k__BackingField")]
    public int QuestionId { get; set; }

    [JsonPropertyName("<RecipientId>k__BackingField")]
    public long RecipientId { get; set; }

    [JsonPropertyName("<UserSalt>k__BackingField")]
    public string? UserSalt { get; set; }

    [JsonPropertyName("<careof>k__BackingField")]
    public string? CareOf { get; set; }

    [JsonPropertyName("<cdesstatprovterrid>k__BackingField")]
    public int CdesStatProvTerrId { get; set; }

    [JsonPropertyName("<city>k__BackingField")]
    public string? City { get; set; }

    [JsonPropertyName("<clstnmsuffixid>k__BackingField")]
    public int ClstnmSuffixId { get; set; }

    [JsonPropertyName("<clstnmtitleid>k__BackingField")]
    public int ClstnmTitleId { get; set; }

    [JsonPropertyName("<ctrycode>k__BackingField")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("<email>k__BackingField")]
    public string? Email { get; set; }

    [JsonPropertyName("<envelopeline>k__BackingField")]
    public string? EnvelopeLine { get; set; }

    [JsonPropertyName("<errormessage>k__BackingField")]
    public string? ErrorMessage { get; set; }

    [JsonPropertyName("<extraaddress1>k__BackingField")]
    public string? ExtraAddress1 { get; set; }

    [JsonPropertyName("<extraaddress2>k__BackingField")]
    public string? ExtraAddress2 { get; set; }

    [JsonPropertyName("<extraaddress3>k__BackingField")]
    public string? ExtraAddress3 { get; set; }

    [JsonPropertyName("<firstname>k__BackingField")]
    public string? FirstName { get; set; }

    [JsonPropertyName("<iaacctno>k__BackingField")]
    public string? IaAcctNo { get; set; }

    [JsonPropertyName("<iaid>k__BackingField")]
    public int IaId { get; set; }

    [JsonPropertyName("<id>k__BackingField")]
    public int Id { get; set; }

    [JsonPropertyName("<indcorinthian>k__BackingField")]
    public string? IndCorinthian { get; set; }

    [JsonPropertyName("<lastmodifiedby>k__BackingField")]
    public string? LastModifiedBy { get; set; }

    [JsonPropertyName("<middlename>k__BackingField")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("<name>k__BackingField")]
    public string? Name { get; set; }

    [JsonPropertyName("<oldaccountnbr>k__BackingField")]
    public string? OldAccountNbr { get; set; }

    [JsonPropertyName("<phone>k__BackingField")]
    public string? Phone { get; set; }

    [JsonPropertyName("<phoneareacode>k__BackingField")]
    public string? PhoneAreaCode { get; set; }

    [JsonPropertyName("<phoneextension>k__BackingField")]
    public int PhoneExtension { get; set; }

    [JsonPropertyName("<postalcode>k__BackingField")]
    public string? PostalCode { get; set; }

    [JsonPropertyName("<qtyrecentdelinquentcancels>k__BackingField")]
    public int QtyRecentDelinquentCancels { get; set; }

    [JsonPropertyName("<qtysubshiftpending>k__BackingField")]
    public int QtySubShiftPending { get; set; }

    [JsonPropertyName("<salutation>k__BackingField")]
    public string? Salutation { get; set; }

    [JsonPropertyName("<street1>k__BackingField")]
    public string? Street1 { get; set; }

    [JsonPropertyName("<street2>k__BackingField")]
    public string? Street2 { get; set; }

    [JsonPropertyName("<ucity>k__BackingField")]
    public string? UCity { get; set; }

    [JsonPropertyName("<uextraaddress1>k__BackingField")]
    public string? UExtraAddress1 { get; set; }

    [JsonPropertyName("<uextraaddress2>k__BackingField")]
    public string? UExtraAddress2 { get; set; }

    [JsonPropertyName("<uextraaddress3>k__BackingField")]
    public string? UExtraAddress3 { get; set; }

    [JsonPropertyName("<ufirstname>k__BackingField")]
    public string? UFirstName { get; set; }

    [JsonPropertyName("<uname>k__BackingField")]
    public string? UName { get; set; }

    [JsonPropertyName("<upostalcode>k__BackingField")]
    public string? UPostalCode { get; set; }

    [JsonPropertyName("<ustreet1>k__BackingField")]
    public string? UStreet1 { get; set; }

    [JsonPropertyName("<ustreet2>k__BackingField")]
    public string? UStreet2 { get; set; }
}
