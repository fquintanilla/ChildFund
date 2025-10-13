namespace ChildFund.Web.Features.Shared.Interfaces;

public interface IContainer : IContentData
{
}


[UIDescriptorRegistration]
public class IContainerDescriptor : UIDescriptor<IContainer>
{
}