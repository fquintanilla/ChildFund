using Schema.NET;

namespace ChildFund.Web.Infrastructure.SchemaMarkup;

/// <summary>
/// Interface for mapping CMS content to Schema.org types
/// </summary>
public interface ISchemaDataMapper<in T> where T : IContent
{
    Thing Map(T content);
}