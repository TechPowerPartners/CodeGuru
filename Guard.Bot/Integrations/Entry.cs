using Guard.Bot.Integrations.GuardApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guard.Bot.Integrations;
internal static class Entry
{
	public static IServiceCollection ConfigureIntergrations(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigureGuardApiIntergration(configuration);

		return services;
	}
}
