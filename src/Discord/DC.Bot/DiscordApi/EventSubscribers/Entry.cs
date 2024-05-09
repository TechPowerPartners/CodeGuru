using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace DC.Bot.DiscordApi.EventSubscribers;
internal static class Entry
{
	public static IServiceCollection ConfigureSubscribersApi(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDiscordGuildMemberAddedEventSubscriber<GuildMemberEventsSubscriberModule>();
		services.AddDiscordComponentInteractionCreatedEventSubscriber<ComponentInteractionCreatedEventSubscriber>();
		services.AddDiscordModalSubmittedEventSubscriber<ModalSubmittedEventSubscriber>();

		return services;
	}
}
