using Telegram.Bot;
using Telegram.Bot.Types;
using TG.Bot.Common.FiniteStateMachine;

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
}
