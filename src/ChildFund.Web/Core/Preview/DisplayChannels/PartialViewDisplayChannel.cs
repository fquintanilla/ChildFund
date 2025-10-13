namespace ChildFund.Web.Core.Preview.DisplayChannels;

public class PartialViewDisplayChannel : DisplayChannel
{
    public const string _partialViewDisplayChannelName = "Partial View Preview";

    public override string ChannelName => _partialViewDisplayChannelName;

    public override bool IsActive(HttpContext context) => false;
}