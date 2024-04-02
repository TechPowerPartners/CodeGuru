using DSharpPlus;
using DSharpPlus.EventArgs;
using Nefarius.DSharpPlus.Extensions.Hosting.Events;

namespace Guard.Bot.SubscriberModules;

public class GuildMemberEventsSubscriberModule : IDiscordGuildMemberAddedEventSubscriber
{
    public async Task DiscordOnGuildMemberAdded(DiscordClient sender, GuildMemberAddEventArgs args)
    {
        var generalChannel = args.Guild.Channels.Values.FirstOrDefault(channel =>
            channel.Name == ChannelNameConsts.General &&
            channel.Type == ChannelType.Text);

        if (generalChannel is null)
        {
            Console.WriteLine("Channel not found");
            return;
        }

        var message = $"""
            ДарооВа @{args.Member.Mention} в наш уютный {args.Guild.Name}!
            Ты шарпист или же геймдейвер?
            Расскажи о себе(если не ответишь кик через 2 минуты)
            """;

        await generalChannel.SendMessageAsync(message);
    }
}