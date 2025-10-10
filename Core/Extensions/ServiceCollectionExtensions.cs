using ChildFund.Core.Auth;
using ChildFund.Core.Http;
using ChildFund.Core.Options;
using Microsoft.Extensions.Options;

namespace ChildFund.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChildFund(this IServiceCollection services, IConfiguration cfg)
        {
            services.Configure<ChildFundOptions>(cfg.GetSection("ChildFund"));

            services.AddMemoryCache();

            services.AddHttpClient("childfund-auth", (sp, http) =>
                {
                    var opts = sp.GetRequiredService<IOptions<ChildFundOptions>>().Value;
                    http.BaseAddress = new Uri(opts.BaseUrl.TrimEnd('/') + "/");
                    http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                })
                .AddPolicyHandler(ChildFundApiClient.DefaultRetryPolicy());

            services.AddHttpClient<IChildFundClient, ChildFundClient>((sp, http) =>
                {
                    var opts = sp.GetRequiredService<IOptions<ChildFundOptions>>().Value;
                    http.BaseAddress = new Uri(opts.BaseUrl.TrimEnd('/') + "/");
                    http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                })
                .AddPolicyHandler(ChildFundApiClient.DefaultRetryPolicy());

            services.AddSingleton<ITokenProvider, TokenProvider>();

            return services;
        }
    }
}
