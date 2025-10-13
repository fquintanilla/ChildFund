namespace ChildFund.Features.Shared.Interfaces
{
	public interface IRestricted : IContentData
    {
    }

    [UIDescriptorRegistration]
    public class IRestrictedDescriptor : UIDescriptor<IRestricted>
    {
    }
}
