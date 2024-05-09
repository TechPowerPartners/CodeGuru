using DC.Bot.DiscordApi.EventSubscribers;
using DC.Bot.DiscordApi.PrefixCommands;
using DC.Bot.DiscordApi.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DC.Bot.DiscordApi;
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
