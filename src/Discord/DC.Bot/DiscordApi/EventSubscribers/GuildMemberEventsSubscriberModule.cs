using DC.Bot.Common;
using DSharpPlus;
using DSharpPlus.EventArgs;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace DC.Bot.DiscordApi.EventSubscribers;

public class GuildMemberEventsSubscriberModule : IDiscordGuildMemberAddedEventSubscriber
{
	public async Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
	{
		var generalChannel = args.Guild.Channels.Values.FirstOrDefault(channel =>
			 channel.Name == ChannelNameConsts.General &&
			 channel.Type == ChannelType.Text);

		var message = $"""
            Добро пожаловать {args.Member.Mention} в наш уютный {args.Guild.Name}!
            Меня зовут Исмаил и я буду твоим наставником.
            Я буду сопровождать тебя на всем пути становления настоящим разработчиком!
            
            Для того чтобы со мной общаться, напиши "!guard help"

            А пока что, расскажи о себе. Какой у тебя опыт? Какую сферу разработки ты выбрал?
            """;

		await generalChannel.SendMessageAsync(message);
	}
}