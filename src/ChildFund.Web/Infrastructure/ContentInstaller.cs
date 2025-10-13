using ChildFund.Web.Infrastructure.Cms.Settings;

namespace ChildFund.Web.Infrastructure
{
    public class ContentInstaller(
        ISettingsService settingsService)
        : IBlockingFirstRequestInitializer
    {
        public bool CanRunInParallel => false;

        public async Task InitializeAsync(HttpContext httpContext)
        {
            InstallDefaultContent(httpContext);
            settingsService.InitializeSettings();
            await Task.CompletedTask;
        }

        private void InstallDefaultContent(HttpContext context)
        {
            ServiceLocator.Current.GetInstance<ISettingsService>().UpdateSettings();
        }
    }
}
