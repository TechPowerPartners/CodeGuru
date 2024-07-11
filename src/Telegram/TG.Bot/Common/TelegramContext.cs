using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Bot.Common.FiniteStateMachine;
using TG.Bot.TelegramApi;

namespace TG.Bot.Common;

internal class TelegramContext(
    ITelegramBotClient botClient,
    Update update,
    CancellationToken сancellationToken,
    IStorage storage
    )
{
    public ITelegramBotClient BotClient { get; set; } = botClient;
    public Update Update { get; set; } = update;
    public CancellationToken CancellationToken { get; set; } = сancellationToken;
    public IStorage Storage { get; set; } = storage;

    public async Task<bool> CheckState(States state, long userId)
    {
        var userStateStr = await Storage.GetState(userId);
        var userState = (States)Enum.Parse(typeof(States), userStateStr!);

        return userStateStr != null && state == userState;
    }
}
