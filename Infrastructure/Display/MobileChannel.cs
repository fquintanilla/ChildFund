using EPiServer.Web;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;

namespace ChildFund.Infrastructure.Display;

public class MobileChannel : DisplayChannel
{
    public override string ChannelName => "Mobile";

    public override string ResolutionId => typeof(IphoneVerticalResolution).FullName ?? string.Empty;

    public override bool IsActive(HttpContext context)
    {
        var detection = context.RequestServices.GetRequiredService<IDetectionService>();
        return detection.Device.Type == Device.Mobile;
    }
}