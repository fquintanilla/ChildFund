namespace ChildFund.Features.Shared.Interfaces
{
	public interface IMasthead : IContentData
    {
        string Heading { get; }
    }

    [UIDescriptorRegistration]
    public class IMastheadDescriptor : UIDescriptor<IMasthead>
    {
    }
}