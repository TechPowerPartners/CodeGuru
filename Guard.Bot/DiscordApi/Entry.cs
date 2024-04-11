using Guard.Bot.DiscordApi.EventSubscribers;
using Guard.Bot.DiscordApi.PrefixCommands;
using Guard.Bot.DiscordApi.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Guard.Bot.DiscordApi;
internal static class Entry
{
	public static IServiceCollection ConfigureDiscordApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.ConfigurePrefixCommandsApi(configuration);
		services.ConfigureSubscribersApi(configuration);
		services.ConfigureSlashCommandsApi(configuration);

		return services;
	}
}
