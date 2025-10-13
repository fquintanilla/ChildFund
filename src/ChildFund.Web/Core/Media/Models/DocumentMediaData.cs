namespace ChildFund.Web.Core.Media.Models;

[ContentType(DisplayName = "Document File",
    GUID = "{95601F30-E915-4514-A971-E0BEA935A60D}",
    Description = "Used for document file types such as PDF, Word, Excel")]
[MediaDescriptor(ExtensionString = "pdf,doc,docx,xsl,xslx,csv")]
public class DocumentMediaData : MediaData
{
    [CultureSpecific]
    [Display(Description = "Description of the document", GroupName = SystemTabNames.Content, Order = 160)]
    public virtual string Description { get; set; }
}