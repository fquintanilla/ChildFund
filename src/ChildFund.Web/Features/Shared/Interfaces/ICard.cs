namespace ChildFund.Web.Features.Shared.Interfaces;

public interface ICard : IContentData
{
}

[UIDescriptorRegistration]
public class ICardDescriptor : UIDescriptor<ICard>
{
}

