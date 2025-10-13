using System.Text.Json;

namespace ChildFund.Web.Core.Http
{
    internal static class JsonDefaults
    {
        public static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };
    }
}
