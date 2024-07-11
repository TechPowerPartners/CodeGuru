using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBotExtension.Types;

namespace TelegramBotExtension.Handling;

[Handler(UpdateType.Message)]
public abstract class MessageHandler : IUpdateTypeHandler
{
    public abstract Task HandleUpdateAsync(TelegramContext context);

    public TelegramContext GetContext(ITelegramBotClient botClient, Update update)
    {
        return new TelegramContext(
            botClient,
            update,
            update.Message!.From!.Id,
            update.Message.Text!
            );
    }
}
