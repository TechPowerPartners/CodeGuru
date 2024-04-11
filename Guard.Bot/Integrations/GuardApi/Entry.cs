using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Guard.Bot.Integrations.GuardApi;
internal static class Entry
{
	public static IServiceCollection ConfigureGuardApiIntergration(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddRefitClient<IGuardApi>()
			.ConfigureHttpClient(client =>
			{
				client.BaseAddress = new(configuration.GetValue<string>("ApiUrl")!);
			});

		return services;
	}
}
