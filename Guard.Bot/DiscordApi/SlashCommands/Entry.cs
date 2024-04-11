using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.SlashCommands.Extensions.Hosting;

namespace Guard.Bot.DiscordApi.SlashCommands;
internal static class Entry
{
	public static IServiceCollection ConfigureSlashCommandsApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDiscordSlashCommands(extension: extension =>
		{

		});

		return services;
	}
}
