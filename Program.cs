using ChildFund.Infrastructure.Cms.Helpers;
using Serilog;

namespace ChildFund;

public class Program
{
	public static IConfiguration Configuration { get; } =
		new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", false, true)
			.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower()}.json",
				true, true)
			.AddEnvironmentVariables()
			.Build();

	public static void Main(string[] args)
	{
		Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Error()
			.ReadFrom.Configuration(Configuration).WriteTo.Console().CreateLogger();

		CreateHostBuilder(args, EnvironmentHelper.IsLocal()).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args, bool isLocal)
	{
		if (isLocal)
		{
			//Development configuration can be added here, like local logging.
			return Host.CreateDefaultBuilder(args)
				.ConfigureCmsDefaults()
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
		}
		else
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureCmsDefaults()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>().UseKestrel(o => o.Limits.MaxRequestBodySize = 419_430_400);
				});
		}
	}
}
