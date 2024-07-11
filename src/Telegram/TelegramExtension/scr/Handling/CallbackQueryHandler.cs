using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBotExtension.Types;

namespace TelegramBotExtension.Handling;

[Handler(UpdateType.CallbackQuery)]
public abstract class CallbackQueryHandler : IUpdateTypeHandler
{
    public abstract Task HandleUpdateAsync(TelegramContext context);

    public TelegramContext GetContext(ITelegramBotClient botClient, Update update)
    {
        return new TelegramContext(
            botClient,
            update,
            update.CallbackQuery!.From.Id,
            update.CallbackQuery.Data!
            );
    }
}
