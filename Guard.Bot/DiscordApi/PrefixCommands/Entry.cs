using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.CommandsNext.Extensions.Hosting;

namespace Guard.Bot.DiscordApi.PrefixCommands;
internal static class Entry
{
	public static IServiceCollection ConfigurePrefixCommandsApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDiscordCommandsNext(
			options =>
			{
				options.StringPrefixes = [configuration.GetValue<string>("BotSettings:CommandPrefix")!];
				options.EnableDms = false;
				options.EnableMentionPrefix = true;
				options.EnableDefaultHelp = false;
			},
			extension =>
			{
				extension.RegisterCommands<CoreCommands>();
				extension.RegisterCommands<EvalCommands>();

			});

		return services;
	}
}
