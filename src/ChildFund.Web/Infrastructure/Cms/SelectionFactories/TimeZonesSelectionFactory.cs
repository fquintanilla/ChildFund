namespace ChildFund.Web.Infrastructure.Cms.SelectionFactories;

public class TimeZonesSelectionFactory : ISelectionFactory
{
    public static readonly string EST = "Eastern Standard Time";
    public static readonly string CST = "Central Standard Time";
    public static readonly string MST = "Mountain Standard Time";
    public static readonly string PST = "Pacific Standard Time";
    public static readonly string AST = "Alaskan Standard Time";
    public static readonly string HST = "Hawaiian Standard Time";

    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata) =>
        new ISelectItem[]
        {
            new SelectItem { Text = "Eastern Standard Time (EST)" , Value = EST},
            new SelectItem { Text = "Central Standard Time (CST)" , Value = CST},
            new SelectItem { Text = "Mountain Standard Time (MST)" , Value = MST},
            new SelectItem { Text = "Pacific Standard Time (PST)" , Value = PST},
            new SelectItem { Text = "Alaskan Standard Time (AST)" , Value = AST},
            new SelectItem { Text = "Hawaiian Standard Time (HST)" , Value = HST},
        };
}
