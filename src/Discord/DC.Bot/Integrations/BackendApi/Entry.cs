using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DC.Bot.Integrations.BackendApi;
internal static class Entry
{
	public static IServiceCollection ConfigureGuardApiIntergration(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddRefitClient<IBackendApi>()
			.ConfigureHttpClient(client =>
			{
				client.BaseAddress = new(configuration.GetValue<string>("ApiUrl")!);
			});

		return services;
	}
}
