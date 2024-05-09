using DC.Bot.Integrations.GuardApi;
using DC.Bot.Integrations.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DC.Bot.Integrations;
internal static class Entry
{
	public static IServiceCollection ConfigureIntergrations(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureGuardApiIntergration(configuration);
		services.ConfigureQueue(configuration);

		return services;
	}
}
