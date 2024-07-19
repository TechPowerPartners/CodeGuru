using DC.Bot.Common;
using DC.Bot.Common.Settings;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.Options;
using Nefarius.DSharpPlus.Extensions.Hosting;

namespace DC.Bot.Integrations.Queue.Consumers;

internal class ArticleCreatedConsumer(
    IDiscordClientService _discordClientService,
    IOptions<DiscordServerSettings> _tppServerSettings) : IConsumeAsync<ArticleCreatedConsumer>
{
    public async Task ConsumeAsync(ArticleCreatedConsumer message, CancellationToken cancellationToken = default)
    {
        var guild = await _discordClientService.Client.GetGuildAsync(_tppServerSettings.Value.Id);

        var articlesChannel = guild.Channels.Values.FirstOrDefault(c => c.Name == ChannelNameConsts.Articles);
        articlesChannel.SendMessageAsync(b =>
        {
            b.
        })
    }
}
